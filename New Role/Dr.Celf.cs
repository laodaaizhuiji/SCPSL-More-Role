using CommandSystem;
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
using static Dr.EventHandler;

namespace Dr
{
    public class Config : IConfig
    {
        [Description("Use Dr.Celf")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Laodaaizhuiji from github";
        public override string Name => "Role of Dr.Clef";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Player.Hurting += handler.Hurting;

        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.Hurting -= handler.Hurting;
        }
    }
    public class EventHandler
    {
        public static List<Player> dr = new List<Player>();
        public static void Spawndr(Player player)
        {
            dr.Add(player);
            player.AddItem(ItemType.GunFRMG0);
            player.AddItem(ItemType.MicroHID);
            player.AddItem(ItemType.KeycardO5);
            player.AddItem(ItemType.SCP1853);
            player.AddItem(ItemType.Medkit);
            player.MaxHealth = 150;
            player.Health = 150;
            player.ShowHint("You are Dr. Clef,Take up to 30 damage, the target of all SCP objects.");
            player.RankColor = "yellow";
            player.RankName = "Dr.Clef";
        }
        public void Hurting(HurtingEventArgs h)
        {
            if (dr.Contains(h.Player) && h.Attacker.IsScp)
            {
                h.Amount = 30;
            }
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn_dr : ICommand
    {
        public string Command { get; set; } = "spawnClef";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "use the command to spawn the Dr.Clef";

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
                Spawndr(p);
                response = "succeed!";
                return true;
            }
        }
    }
}
