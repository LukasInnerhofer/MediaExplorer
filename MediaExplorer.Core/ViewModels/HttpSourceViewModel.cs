using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public class HttpSourceViewModel : MvxViewModel
    {
        private List<string> _supportedFileExtensions = new List<string>()
        {
            "jpeg", "jpg",
            "png",
            "bmp",
            "gif",
            "tiff",
            "ico",
            "svg",
            "mp4",
            "mpeg",
            "webm"
        };

        private string _url;
        public string Url 
        { 
            get
            {
                return _url;
            }
            set
            {
                SetProperty(ref _url, value);
                if(_url.Contains("."))
                {
                    string fileExtension = _url.Split('.').Last();
                    if (_supportedFileExtensions.Contains(fileExtension))
                    {
                        FileExtension = fileExtension;
                    }
                }
            }
        }

        private string _fileExtension;
        public string FileExtension 
        { 
            get
            {
                return _fileExtension;
            }
            set
            {
                SetProperty(ref _fileExtension, value);
            }
        }

        public HttpSourceViewModel(string url)
        {
            FileExtension = string.Empty;
            Url = url;
        }
    }
}
