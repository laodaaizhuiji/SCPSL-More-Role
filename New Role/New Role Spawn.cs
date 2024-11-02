using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scp008.EventHandler;
using static Scp3035.EventHandler;
using static Scp999.EventHandler;
using static Scp181.EventHandler;
using static Dr.EventHandler;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.ComponentModel;
using MEC;

namespace New_Role
{
    public class Config : IConfig
    {
        [Description("open role spawn")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "laodaaizhuiji from github";
        public override string Name => "role spawn";
        private EventHandler handler;
        public override void OnEnabled()
        {
            base.OnEnabled();
            handler = new EventHandler();
            Exiled.Events.Handlers.Server.RoundStarted += handler.stRound;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Server.RoundStarted -= handler.stRound;
        }
    }
    public class EventHandler
    {
        public void stRound()
        {
            foreach (Player player in Player.List)
            {
                if (scp181.Count < 1 && player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                {
                    Spawn181(player);
                }
                if (scp999.Count < 1 && Player.List.Count > 5 && player.Role.Type == PlayerRoles.RoleTypeId.FacilityGuard)
                {
                    Spawn999(player);
                }
                if (scp3035.Count < 1 && Player.List.Count > 9  && player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
                {
                    Spawn3035(player);
                }
                if (dr.Count < 1 && Player.List.Count > 11 && player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
                {
                    Spawndr(player);
                }
                if (scp008.Count < 1 && Player.List.Count > 12 && player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
                {
                    Spawn008(player);
                }
            }
        }
    }
}
