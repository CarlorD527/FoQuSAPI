using AutoMapper;
using FQ.Application.Bases;
using FQ.Application.Dtos.Posts;
using FQ.Application.Interfaces;
using FQ.Application.Validators.Posts;
using FQ.Domain.Entities;
using FQ.Infrastructure.Interfaces;
using FQ.Infrastructure.Repositories;
using Google.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;
using FQ.Utilities.Static;
using FQ.Infrastructure.AzureService;
using Microsoft.AspNetCore.Mvc;
using static Grpc.Core.Metadata;
using FQ.Infrastructure.FirestoreServices;

namespace FQ.Application.Services
{
    public class PostApplication : IPostsApplication
    {
        private readonly PostRepository postRepository;

        private readonly IMapper _mapper;

        private readonly PostValidators _validatorRules;

        private readonly ContentModeratorService _contentModeratorService;
        public PostApplication(PostRepository _postrepository,
            IMapper mapper, PostValidators validatorRules, ContentModeratorService contentModerator)
        {
            postRepository = _postrepository;
            _mapper = mapper;
            _validatorRules = validatorRules;
            _contentModeratorService = contentModerator;
        
        }
        public async Task<BaseResponse<List<Post>>> GetAllPosts()
        {
            var response = new BaseResponse<List<Post>>();
            var posts = await postRepository.GetAllAsync();


            if (posts is not null)
            {
                response.isSuccess = true;
                response.Data = posts.OrderByDescending(p=>p.fechaCreacion).ToList();
                response.Message = ReplyMessages.MESSAGE_QUERY;
            }
            else
            {

                response.isSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }
        public async Task<BaseResponse<List<Post>>> GetByIdPost(string id)
        {
            var response = new BaseResponse<List<Post>>();

            var posts = await postRepository.GetByIdAsync(id);

            if (posts is not null)
            {
                response.isSuccess = true;
                response.Data = posts;
                response.Message = ReplyMessages.MESSAGE_QUERY;
            }
            else
            {

                response.isSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> addPost([FromForm] AddPostDto postDto)
        {
            var response = new BaseResponse<bool>();

            //Llamar metodo para realizar las validaciones basicas
            var validationResult = await _validatorRules.ValidateAsync(postDto);

            ////<------ POR IMPLEMENTAR ----->
            //var moderationTextResult =  _contentModeratorService.ModerateTextAsync(postDto.Contenido!);

            FireStoreUTILS fireStoreUTILS = new FireStoreUTILS();

            //llamar metodo para subir imagen al firestore
            var imagenUrl = await fireStoreUTILS.UploadImage(postDto.Imagen!, postDto.Imagen!.FileName);


            //llamar metodo que nos da el resultado de la evaluacion de la imagen
            var moderationImageResult = _contentModeratorService.ModerateImageAsync(postDto.Imagen!);

            //Adjuntar el resultando en el response
            response.moderationImageResult = moderationImageResult.Result;

            //response.moderationTextResult = moderationTextResult.Result;
            //Validaciones basicas  y validacion del resultado del evaluador cognitivo
            if (!validationResult.IsValid || moderationImageResult.Result.IsImageAdultClassified == true || moderationImageResult.Result.IsImageRacyClassified == true)
            {
           
                await fireStoreUTILS.DeleteImage(postDto.Imagen!.FileName);
                response.isSuccess = false;
                response.Message = ReplyMessages.MESSAGE_FAILED;
                response.Errors = validationResult.Errors;
            }
            else
            {
                //Mapeo del postDto al post
                var post = _mapper.Map<Post>(postDto);

                post.fechaCreacion = DateTime.UtcNow;
                post.UrlImagen = imagenUrl;
                bool isPostAdded;

                try
                {
                //Registrar Post
                    await postRepository.PostAsync(post);
                    isPostAdded = true;
                }
                catch (Exception)
                {
                    isPostAdded = false;
                }

                //Adjuntar mensajes de respuesta al response (error, succeso, objeto guardado)
                if (isPostAdded)
                {
                    response.isSuccess = true;
                    response.Message = ReplyMessages.MESSAGE_SAVE;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = ReplyMessages.MESSAGE_FAILED;
                }
            }

            return response;
        }

        public async Task<BaseResponse<bool>> deletePost(string id)
        {
            var response = new BaseResponse<bool>();

 
            //trayendo la lista de post (tiene un solo post)
            var Listapost = await postRepository.GetByIdAsync(id);

            var post = Listapost.FirstOrDefault(p=>p.Id == id);


            FireStoreUTILS fireStoreUTILS = new FireStoreUTILS();

            await fireStoreUTILS.DeleteImageByUrl(post?.UrlImagen!);


            bool isDeleted = await postRepository.DeleteByIdAsync(id);

            if (isDeleted)
            {
                response.isSuccess = true;
                response.Data = isDeleted;
                response.Message = "Post eliminado correctamente!";
            }
            else
            {
                response.isSuccess = false;
                response.Message = "Error al eliminar un post.";
            }

            return response;
        }
    }
}
