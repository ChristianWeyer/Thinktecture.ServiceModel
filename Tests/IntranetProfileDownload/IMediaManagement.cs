using System.ServiceModel;

namespace MediaContracts
{
    [ServiceContract] 
    public interface IMediaManagement
    {
        [OperationContract]
        AllEpisodesResponse ListAllEpisodes(AllEpisodesRequest request);
    }
}
