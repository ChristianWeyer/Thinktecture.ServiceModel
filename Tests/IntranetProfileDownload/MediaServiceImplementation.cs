using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MediaContracts;

namespace MediaService
{
    public class MediaServiceImplementation
        : IMediaManagement
    {
        public AllEpisodesResponse ListAllEpisodes(
            AllEpisodesRequest request)
        {
            // NOTE:This is hard-coded nonsense just for
            // demo purposes

            string path = 
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string file = Path.Combine(path, "pic.jpg");
            Image im = Bitmap.FromFile(file);

            AllEpisodesResponse response =
                new AllEpisodesResponse();

            List<Episode> episodes = new List<Episode>(3);
            Episode e1 = new Episode();
            e1.ID = 1;
            e1.Title = "Hitchiker's Guide....";
            e1.Expert = "Douglas Adams";
            e1.Description = "Well known!";
            e1.Screenshot = (Bitmap)im;

            episodes.Add(e1);
            response.AllEpisodes = episodes;

            return response;
        }
    }
}
