using System.Threading.Tasks;

namespace TPI
{
    internal interface ICanShow
    {
        Task<string> Show();
    }

    internal interface ICanShow<T> : ICanShow
    {
        new Task<T> Show();
    }

    internal interface ICanSet
    {

    }

    internal interface ICanReset
    {

    }

    internal interface ICanEnable
    {

    }

    internal interface ICanDisable
    {

    }

    internal interface ICanDelete
    {

    }

    internal interface ICanDownload
    {

    }

    internal interface ICanUpload
    {

    }
}
