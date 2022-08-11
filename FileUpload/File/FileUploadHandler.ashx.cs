using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace FileUpload
{
    public class FileUploadHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            db_ppl Connstring = new db_ppl();

            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection UploadedFilesCollection = context.Request.Files;

                for (int i = 0; i < UploadedFilesCollection.Count; i++)
                {
                    //string fileName = UploadedFilesCollection[i].FileName;
                    string extension = Path.GetExtension(UploadedFilesCollection[i].FileName);

                    if (extension == ".pdf")
                    {
                        int result = 0;
                        if (HttpContext.Current.Session["FileID"] != null)
                        {
                            string fileName = HttpContext.Current.Session["FileID"] + extension;

                            HttpPostedFile PostedFiles = UploadedFilesCollection[i];
                            string FilePath = context.Server.MapPath("../FileArchive/" + fileName);
                            PostedFiles.SaveAs(FilePath);
                        }
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}