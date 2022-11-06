using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ToDoAppNTier.Business.Interfaces;
using ToDoAppNTier.Business.ValidationRules;
using ToDoAppNTier.Common.ResponseObjects;
using ToDoAppNTier.DataAccess.UnitOfWork;
using ToDoAppNTier.DTOS.Interfaces;
using ToDoAppNTier.DTOS.WorkDtos;
using ToDoAppNTier.Entities.Domains;

namespace ToDoAppNTier.Business.Services
{
    public class WorkService : IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<WorkCreateDto> _createDtoValidator;
        private readonly IValidator<WorkUpdateDto> _updateDtoValidator;


        public WorkService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<WorkCreateDto> createDtoValidator, IValidator<WorkUpdateDto> updateDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
        }

        public async Task<IResponse<WorkCreateDto>> Create(WorkCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);

            if (validationResult.IsValid)
            {
                await _unitOfWork.GetRepository<Work>().Create(_mapper.Map<Work>(dto));
                await _unitOfWork.SaveChanges();
                return new Response<WorkCreateDto>(ResponseType.Success, dto);

            }
            else
            {
                List<CustomValidationError> errors = new();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(new()
                    {
                        ErrorMessage=error.ErrorMessage,
                        PropertyName=error.PropertyName,
                    });

                }
                return new Response<WorkCreateDto>(ResponseType.ValidationError,dto,errors);
            }

        }

        public async Task<IResponse<List<WorkListDto>>> GetAll()
        {
            var data = _mapper.Map<List<WorkListDto>>(await _unitOfWork.GetRepository<Work>().GetAll());
            return new Response<List<WorkListDto>>(ResponseType.Success, data);
        }

        public async Task<IResponse<IDto>> GetById<IDto>(int id)
        {
            var data = _mapper.Map<IDto>(await _unitOfWork.GetRepository<Work>().GetByFilter(x => x.Id == id));
            if (data==null)
            {
                return new Response<IDto>(ResponseType.NotFound, $"{id} ait data bulunamadı"); 

            }
            return new Response<IDto>(ResponseType.Success, data);
        }

        public async Task<IResponse> Remove(int id)
        {
            var removedEntity = await _unitOfWork.GetRepository<Work>().GetByFilter(x => x.Id == id);
            if (removedEntity != null)
            {
                _unitOfWork.GetRepository<Work>().Remove(removedEntity);
                await _unitOfWork.SaveChanges();
                return new Response(ResponseType.Success);
            }
            return new Response(ResponseType.NotFound, $"{id} ait data bulunamadı");

        }

        public async Task<IResponse<WorkUpdateDto>> Update(WorkUpdateDto dto)
        {
            var result = _updateDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var updatedEntity = await _unitOfWork.GetRepository<Work>().Find(dto.Id);
                if (updatedEntity != null)
                {
                    _unitOfWork.GetRepository<Work>().Update(_mapper.Map<Work>(dto), updatedEntity);
                    await _unitOfWork.SaveChanges();
                    return new Response<WorkUpdateDto>(ResponseType.Success, dto);
                }
                return new Response<WorkUpdateDto>(ResponseType.NotFound, $"{dto.Id} ait data bulunamadı");

            }
            else
            {
                List<CustomValidationError> errors = new();
                foreach (var error in result.Errors)
                {
                    errors.Add(new()
                    {
                        ErrorMessage = error.ErrorMessage,
                        PropertyName = error.PropertyName,
                    });

                }
                return new Response<WorkUpdateDto>(ResponseType.ValidationError, dto, errors);
            }

        }
    }
}
