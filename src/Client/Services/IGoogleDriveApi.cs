using BiDegree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IGoogleDriveApi
    {
        Task<DriveFileList> GetDriveFileList(string folderId);
        DriveFileList GetFileList();
    }
}
