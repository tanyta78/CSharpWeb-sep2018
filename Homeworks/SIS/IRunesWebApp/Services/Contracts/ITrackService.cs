namespace IRunesWebApp.Services.Contracts
{
    using Models;

    public interface ITrackService
    {
        bool CreateTrack(Track track, string albumId);

        Track GetTrackById(string id);
    }
}
