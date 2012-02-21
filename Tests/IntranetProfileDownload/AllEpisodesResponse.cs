using System.Collections.Generic;
using System.ServiceModel;

namespace MediaContracts
{
    [MessageContract(IsWrapped=false)]
    public class AllEpisodesResponse
    {
        [MessageBodyMember]
        public List<Episode> AllEpisodes;
    }
}
