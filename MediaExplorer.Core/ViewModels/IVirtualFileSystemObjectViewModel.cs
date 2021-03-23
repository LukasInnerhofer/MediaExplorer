using MvvmCross.ViewModels;
using MvvmCross.Commands;

namespace MediaExplorer.Core.ViewModels
{
    public interface IVirtualFileSystemObjectViewModel
    {
        IMvxCommand StartRenameCommand { get; }
    }
}
