using AutoMapper;
using FluentFTP.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.FTP;
using MajlesMefa.Back.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateSenatorProfileCommand
{
    public class UpdateSenatorProfileCommandHandler : IRequestHandler<UpdateSenatorProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _baseLocation;
        //private readonly IFtpFileService _ftpFileService;
        private readonly IFileService _fileService;

        public UpdateSenatorProfileCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _baseLocation = configuration.GetSection("FileServer:baseFileLocation").Value;
        }

        public async Task Handle(UpdateSenatorProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.SenatorRepository
                .FindAsync(request.UserId);

            var folderIsExist = _fileService.FileExist(_baseLocation);
            var fileExtension = "";
            var fileName = "";

            if (folderIsExist == false)
            {
                Directory.CreateDirectory(_baseLocation);
            }
            if (request.ProfilePhotoFile is not null)
            {
                fileExtension = Path.GetExtension(request.ProfilePhotoFile.FileName);
                fileName = Guid.NewGuid().ToString("N").ToUpper().Trim() + fileExtension;
                var url = Path.Combine(Directory.GetCurrentDirectory(), _baseLocation, fileName);
                List<string> validFormats = new List<string>() { "image/png", "image/jpg", "image/jpeg", "image/jfif", "image/svg" };
                var isExist = _fileService.FileExist(url);
                if (request.ProfilePhotoFile.Length > 500000)
                {
                    throw new Exception("سایز فایل نمیتواند بیشتر از 5 مگابایت باشد");
                }
                if (!validFormats.Contains(request.ProfilePhotoFile.ContentType))
                {
                    throw new Exception("فرمت فایل انتخابی نادرست است.");
                }
                if (isExist != true)
                {
                    using (var fileStream = new FileStream(url, FileMode.Create))
                    {
                        await request.ProfilePhotoFile.CopyToAsync(fileStream);

                    }
                }


                if (entity is not null && entity.ProfilePhoto is not null)
                {
                    var currentFileExtension = Path.GetExtension(entity.ProfilePhoto);
                    var CurrentFileName = Guid.NewGuid().ToString("N").ToUpper().Trim() + currentFileExtension;
                    var CurrentUrl = Path.Combine(Directory.GetCurrentDirectory(), _baseLocation, CurrentFileName);
                    var CurrentIsExist = _fileService.FileExist(CurrentUrl);
                    if (CurrentIsExist == true)
                    {
                        _fileService.DeleteFileFromServer(CurrentUrl);
                    }
                }

            }




            //var physicalFileName = "";
            //try
            //{
            //    var fileContentType = request.ProfilePhoto.ContentType.ToLower();
            //    var fileExtension = Path.GetExtension(request.ProfilePhoto.FileName);
            //    var fileSize = request.ProfilePhoto.Length; // in bytes
            //    var fileHash = FileHelper.CalculateFileHash(request.ProfilePhoto);
            //    physicalFileName = Guid.NewGuid().ToString("N").ToUpper().Trim() + fileExtension;
            //    var path = Path.Combine("Senator", physicalFileName);

            //    if (request.ProfilePhoto is null || request.ProfilePhoto.Length <= 0)
            //        throw new Exception("انتخاب عکس پروفایل نماینده اجباری میباشد.");

            //    planFile.SetPath(physicalFileName, path, fileExtension, fileContentType, fileSize, fileHash);

            //    await using var memoryStream = new MemoryStream();

            //    memoryStream.Position = 0;
            //    await request.ProfilePhoto.CopyToAsync(memoryStream);

            //    memoryStream.Position = 0;

            //    await _ftpFileService.UploadAsync(memoryStream, path, cancellationToken);
            //}
            //catch (Exception ex)
            //{
            //    if (request.ProfilePhoto is null || request.ProfilePhoto.Length <= 0)
            //        throw new Exception("فایل انتخاب شده خالی میباشد");
            //    else
            //        await _ftpFileService.DeleteFileAsync(Path.Combine("Senator", physicalFileName), cancellationToken);

            //}

            
            

            if (entity == null)
            {
                entity = new SenatorProfileEntity();
                entity.ProfilePhoto = fileName;
                _unitOfWork.SenatorRepository.Add(entity);
            }

            var lastImg = entity.ProfilePhoto;
            var lastId = entity.PersonalFavorites;
            var import = entity.FractionMembership;
            entity = _mapper.Map(request, entity);
            if (entity is not null && entity.ProfilePhoto is not null)
            {
                entity.ProfilePhoto = fileName;
            }
            entity.PersonalFavorites = lastId;
            entity.FractionMembership = import;
            //entity.HozeCityId = null;
            if (request.ProfilePhotoFile is null)
            {
                entity.ProfilePhoto = lastImg != "" ? lastImg : null;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);




        }
    }
}
