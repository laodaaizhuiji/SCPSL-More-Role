using CommandSystem;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scp008.EventHandler;

namespace Scp008
{
    public class Config : IConfig
    {
        [Description("Use Scp008")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Laodaaizhuiji from github";
        public override string Name => "Role of Scp008";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Player.Dying += handler.Dying;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.Dying -= handler.Dying;
        }
    }
    public class EventHandler
    {
        public static List<Player> scp008 = new List<Player>();
        public static void Spawn008(Player player)
        {
            player.Role.Set(PlayerRoles.RoleTypeId.Scp0492);
            player.MaxHealth = 2500;
            player.Health = 2500;
            player.RankColor = "pink";
            player.RankName = "SCP999";
            scp008.Add(player);
            player.ShowHint("You are Scp008,You can run very fast");
            player.Teleport(RoomType.Hcz096);
            player.EnableEffect<Scp207>();
        }
        public void Dying(DyingEventArgs d)
        {
            d.Player.MaxHealth = 100;
            scp008.Remove(d.Player);
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn_008 : ICommand
    {
        public string Command { get; set; } = "spawn008";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "use the command to spawn the scp008";

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
                Spawn008(p);
                response = "succeed!";
                return true;
            }
        }
    }
}
