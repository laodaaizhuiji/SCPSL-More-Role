using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using CommandSystem;
using static Scp3035.EventHandler;

namespace Scp3035
{
    public class Config : IConfig
    {
        [Description("Use Scp3035")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Laodaaizhuiji from github";
        public override string Name => "Role of scp3035";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Player.Dying += handler.Dying;
            Exiled.Events.Handlers.Player.Hurting += handler.Hurting;
            Exiled.Events.Handlers.Player.Escaping += handler.Escap;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.Dying -= handler.Dying;
            Exiled.Events.Handlers.Player.Hurting -= handler.Hurting;
            Exiled.Events.Handlers.Player.Escaping -= handler.Escap;
        }
    }
    public class EventHandler
    {
        public static List<Player> scp3035 = new List<Player>();
        public static void Spawn3035(Player player)
        {
            player.VoiceChannel = VoiceChat.VoiceChatChannel.ScpChat;
            scp3035.Add(player);
            player.AddItem(ItemType.KeycardO5);
            player.ShowHint("You are scp3035.If you escape, you can turn into a random SCP unit");
        }
        public void Dying(DyingEventArgs d)
        {
            if (scp3035.Contains(d.Player))
            {
                d.Player.RankName = null;
                d.Player.RankColor = null;
                scp3035.Remove(d.Player);
                Cassie.MessageTranslated("s c p 3 0 3 5 contains successfully", "scp-3035 contains successfully");
            }
        }
        public void Hurting(HurtingEventArgs h)
        {
            if (scp3035.Contains(h.Player) && h.Attacker.IsScp)
            {
                h.IsAllowed = false;
                h.Attacker.Broadcast(3, "<color=#00CED1>Don't attack players on the same faction");
            }
        }
        public void Escap(EscapingEventArgs e)
        {
            int escap = new System.Random().Next(1, 3);
            if (escap == 1)
            {
                e.IsAllowed = false;
                e.Player.Role.Set(PlayerRoles.RoleTypeId.Scp096);
                e.Player.Broadcast(4, "You escaped and became SCP096");
            }
            if (escap == 2)
            {
                e.IsAllowed = false;
                e.Player.Role.Set(PlayerRoles.RoleTypeId.Scp049);
                e.Player.Broadcast(4, "You escaped and became SCP049");
            }
            if (escap == 3)
            {
                e.IsAllowed = false;
                e.Player.Role.Set(PlayerRoles.RoleTypeId.Scp173);
                e.Player.Broadcast(4, "You escaped and became SCP173");
            }
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn_3035 : ICommand
    {
        public string Command { get; set; } = "spawn3035";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "use the command to spawn the scp3035";

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
                Spawn3035(p);
                response = "succeed!";
                return true;
            }
        }
    }
}
