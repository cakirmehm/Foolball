using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Foolball
{
    public partial class F_Info : Form
    {
        CPlayer selectedPlayer = null;

        public F_Info()
        {
            InitializeComponent();
        }

        public void fillPlayerInfo(CPlayer player)
        {

            lblPosition.Text = player.Position.ToString();
            lblTotalDistance.Text = player.TotalDistance.ToString();
            lblGoalScored.Text = player.nOfGoal.ToString();
            lblnOfShoots.Text = player.nOfShoot.ToString();
            lblnOfPasses.Text = player.nOfPass.ToString();
            lblnOfDribblings.Text = player.nOfDribble.ToString();

            tbarSpeed.Value = (int) player.Speed;
            tbarPriority.Value = (int)player.Priority;
            pbarEnergy.Value = player.Energy;
            tbarPower.Value = (int) player.Power;
            chkAlways.Checked = player.blnAlways;

            selectedPlayer = player;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            selectedPlayer.Priority = (EPriority) tbarPriority.Value;
            selectedPlayer.Speed = (ESpeed)tbarSpeed.Value;
            selectedPlayer.Power = (EPower)tbarPower.Value;

            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAlways_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedPlayer != null)
                selectedPlayer.blnAlways = chkAlways.Checked;
        }


    }
}