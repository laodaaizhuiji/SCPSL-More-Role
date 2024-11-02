using CommandSystem;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Scp999.EventHandler;

namespace Scp999
{
    public class Config : IConfig
    {
        [Description("Use Scp999")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Laodaaizhuiji from github";
        public override string Name => "Role of scp999";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Player.ChangingItem += handler.item;
            Exiled.Events.Handlers.Player.Dying += handler.Dying;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.ChangingItem -= handler.item;
            Exiled.Events.Handlers.Player.Dying -= handler.Dying;
        }
    }
    public class EventHandler
    {
        public static bool zt = false;
        public static List<Player> scp999 = new List<Player>();
        public static void Spawn999(Player player)
        {
            player.Role.Set(PlayerRoles.RoleTypeId.Tutorial);
            player.MaxHealth = 9999;
            player.Health = 9999;
            player.Scale /= 4;
            player.AddItem(ItemType.GunFRMG0);
            player.AddItem(ItemType.Flashlight);
            player.AddItem(ItemType.ArmorHeavy);
            player.AddItem(ItemType.KeycardO5);
            player.RankColor = "pink";
            player.RankName = "SCP999";
            scp999.Add(player);
            player.ShowHint("You are scp-999,Holding a flashlight can regenerate blood to those around you.");
            player.Teleport(RoomType.LczArmory);
            player.EnableEffect<Scp207>();
        }
        public void Dying(DyingEventArgs d)
        {
            if (scp999.Contains(d.Player))
            {
                d.Player.RankName = null;
                d.Player.RankColor = null;
                scp999.Remove(d.Player);
                Cassie.MessageTranslated("s c p 9 9 9 contains successfully", "scp-999 contains successfully");
            }
        }
        public IEnumerator<float> 回血()
        {
            for (; ; )
            {
                if (zt)
                {
                    foreach (Player p in Player.List)
                    {
                        if (Vector3.Distance(p.Position, scp999[0].Position) <= 10 && p != scp999[0])
                        {
                            p.Heal(5);
                            scp999[0].ShowHitMarker();
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }
        public void item(ChangingItemEventArgs c)
        {
            if (scp999.Contains(c.Player) && c.Item.Type == ItemType.Flashlight)
            {
                zt = true;
            }
            else if (scp999.Contains(c.Player) && c.Item.Type != ItemType.Flashlight)
            {
                zt = false;
            }
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn_999 : ICommand
    {
        public string Command { get; set; } = "spawn999";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "use the command to spawn the scp999";

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
                Spawn999(p);
                response = "succeed!";
                return true;
            }
        }
    }
}
