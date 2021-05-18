using Microsoft.AspNetCore.Http;
using RestaurantApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.Core.Interface
{
    public interface IFileManager<File> where File : class
    {
        public OperationResult UploadFile(File dbEntity, IFormFile originalFile);
        public OperationResult DeleteFile(int id);
        public OperationResult DeleteFile(File dbEntity);
        public IEnumerable<File> GetAllFiles(Expression<Func<File, bool>> predicate = null,
                                             Func<IQueryable<File>, IOrderedQueryable<File>> orderBy = null);

        public File GetFileById(int id);
    }
}
