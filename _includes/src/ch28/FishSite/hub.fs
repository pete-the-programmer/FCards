namespace FishSite
open System.Threading.Tasks
open Microsoft.AspNetCore.SignalR

type FishHub() = 
  inherit Hub()

  member this.JoinGame (username: string) =
    task{
      printfn "%s: JoinGame" username
      this.Context.Items.["PlayerName"] <- username
      return! this.Clients.All.SendAsync("PlayerJoined", username, System.Threading.CancellationToken.None)
    }
