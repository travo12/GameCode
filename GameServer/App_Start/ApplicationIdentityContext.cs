
using AspNet.Identity.MongoDB;
using Planc.Dal;
using MongoDB.Driver;
using System;

public class ApplicationIdentityContext : IdentityContext, IDisposable
{
    public ApplicationIdentityContext(MongoCollection users, MongoCollection roles)
        : base(users, roles)
    {
    }

    public static ApplicationIdentityContext Create()
    {
        // todo add settings where appropriate to switch server & database in your own application
        var client = new MongoClient(GameConstants.ConnectionString);
        var database = client.GetServer().GetDatabase(GameConstants.DatabaseName);
        var users = database.GetCollection<IdentityUser>(GameConstants.UserCollectionName);
        var roles = database.GetCollection<IdentityRole>(GameConstants.RoleCollectionName);
        return new ApplicationIdentityContext(users, roles);
    }

    public void Dispose()
    {
    }
}
