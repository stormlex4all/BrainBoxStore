namespace BrainBox.Repositories.Contracts
{
    public interface IBrainBoxUserRepository
    {
        /// <summary>
        /// Gets untracked user Id
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GetUntrackedUserByEmail(string email);
    }
}
