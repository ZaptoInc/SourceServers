Imports Newtonsoft.Json.Linq

Public Interface InteractionsExecutor
    Function Run(obj As DiscordSlashInteraction) As JObject
End Interface
