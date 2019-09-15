using System.Threading.Tasks;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public interface IStreamlabsEvent {

        EventValueParser Parser { get; }

        Task Process();

    }

}
