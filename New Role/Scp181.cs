using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Scp181.EventHandler;

namespace Scp181
{
    public class Config : IConfig
    {
        [Description("Use Scp181")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Laodaaizhuiji from github";
        public override string Name => "Role of Scp999";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Player.InteractingDoor += handler.Inter;
            Exiled.Events.Handlers.Player.Hurting += handler.Hurting;
            Exiled.Events.Handlers.Player.Dying += handler.Dying;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.InteractingDoor -= handler.Inter;
            Exiled.Events.Handlers.Player.Hurting -= handler.Hurting;
            Exiled.Events.Handlers.Player.Dying -= handler.Dying;
        }
    }
    public class EventHandler
    {
        public static List<Player> scp181 = new List<Player>();
        public void Inter(InteractingDoorEventArgs i)
        {
            if (scp181.Contains(i.Player) && i.Door.Type != Exiled.API.Enums.DoorType.Scp079First || i.Door.Type != Exiled.API.Enums.DoorType.Scp079Second)
            {
                int door = new System.Random().Next(1, 5);
                if (door == 1)
                {
                    i.IsAllowed = true;
                    i.Player.Broadcast(3, "You're lucky enough to open a door");
                }
            }
        }
        public void Hurting(HurtingEventArgs h)
        {
            if (scp181.Contains(h.Player))
            {
                int luck = new System.Random().Next(1, 5);
                if (luck == 1)
                {
                    h.IsAllowed = false;
                }
            }
        }
        public void Dying(DyingEventArgs d)
        {
            if (scp181.Contains(d.Player))
            {
                d.Player.RankName = null;
                d.Player.RankColor = null;
                scp181.Remove(d.Player);
                Cassie.MessageTranslated("s c p 1 8 1 contains successfully", "scp-181 contains successfully");
            }
        }
        public static void Spawn181(Player player)
        {
            scp181.Add(player);
            player.MaxHealth = 150;
            player.Health = 150;
            player.ShowHint("You are Scp181,You have a chance to be immune to damage,You can open the door directly.");
            player.RankColor = "orange";
            player.RankName = "SCP181";
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn_181 : ICommand
    {
        public string Command { get; set; } = "spawn181";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "use the command to spawn the scp181";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(arguments.At(0));
            if (p == null)
            {
                response = "can't find the player";
                return false;
            }
            else
            {
                Spawn181(p);
                response = "succeed!";
                return true;
            }
        }
    }
}