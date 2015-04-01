using System.Threading.Tasks;

namespace TPI
{
    public interface ICanShow
    {
        Task<string> Show();
    }

    public interface ICanShow<T> : ICanShow
    {
        new Task<T> Show();
    }

    public interface ICanSet
    {

    }

    public interface ICanReset
    {

    }

    public interface ICanEnable
    {

    }

    public interface ICanDisable
    {

    }

    public interface ICanDelete
    {

    }

    public interface ICanDownload
    {

    }

    public interface ICanUpload
    {

    }
}
