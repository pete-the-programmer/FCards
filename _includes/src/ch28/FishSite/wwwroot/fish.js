var connection = new signalR.HubConnectionBuilder().withUrl("/fishHub").build()
var myName = ""

logMessage = message => {
  console.log(message)
  const item = document.createElement("li")
  item.innerText = message
  document.getElementById("messages").appendChild(item)
}

connection.start()
  .then( () => {
    console.log("Started")
  })
  .catch(err => {
    return console.error(err.toString())
  })

connection.on("PlayerJoined", function (who) {
  logMessage(`${who} joined the game`)
  if(who == myName) {
    document.body.classList.add("joined")
  }
})

const joinGame = (playerName) => {
  myName = playerName
  connection
    .invoke("JoinGame", playerName)
    .catch(function (err) {
      return console.error(err.toString())
    })
}
