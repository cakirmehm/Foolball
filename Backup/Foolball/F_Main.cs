using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Foolball
{
    public partial class F_Main : Form
    {
        private static F_Main fMain = null;
        public static F_Main getInstance()
        {
            if (fMain == null)
                fMain = new F_Main();
            return fMain;
        }

       


        private List<PictureBox> lstPlayersLeft = new List<PictureBox>();
        private List<PictureBox> lstPlayersRight= new List<PictureBox>();

        private List<Point> lstOrgListOfPlayersLeft = new List<Point>();
        private List<Point> lstOrgListOfPlayersRight = new List<Point>();

        private CPlayer selectedPlayerAtThisMoment = null;
        private PictureBox pbxUser = null;


        CPlayer pLeftGK = new CPlayer();
        CPlayer pLeftLB = new CPlayer();
        CPlayer pLeftRB = new CPlayer();
        CPlayer pLeftDMF = new CPlayer();
        CPlayer pLeftLF = new CPlayer();
        CPlayer pLeftRF = new CPlayer();


        CPlayer pRightGK = new CPlayer();
        CPlayer pRightLB = new CPlayer();
        CPlayer pRightRB = new CPlayer();
        CPlayer pRightDMF = new CPlayer();
        CPlayer pRightLF = new CPlayer();
        CPlayer pRightRF = new CPlayer();


        public Dictionary<PictureBox, CPlayer> dctPbxVSPlayer = new Dictionary<PictureBox, CPlayer>();


        Timer tmrGame = new Timer();
        Timer tmrBall = new Timer();
        Timer tmrShoot = new Timer();
        Timer tmrPass = new Timer();
        Timer tmrGoalScored = new Timer();
        Timer tmrEnergyLoss = new Timer();

        bool blnLeftHasTheBall = false, blnRightHasTheBall = false, blnGoalScored = false;
        int ballDevX = 6, ballDevY = 6;
        int leftScored = 0, rightScored = 0;
        int iValTmrGoalScored = 0, iValTmrBall = 0;
        ulong iValTmrEnergyLoss = 0;
        int FSK = 10;

        public F_Main()
        {
            InitializeComponent();



            F_Settings fSettings = new F_Settings();
            fSettings.ShowDialog();

            pLeftGK = fSettings.pLeftGK;
            pLeftLB = fSettings.pLeftLB;
            pLeftRB = fSettings.pLeftRB;
            pLeftDMF = fSettings.pLeftDMF;
            pLeftLF = fSettings.pLeftLF;
            pLeftRF = fSettings.pLeftRF;

            
            tmrGame.Interval = 30;
            tmrGame.Tick += new EventHandler(tmrGame_Tick);
            tmrGame.Start();


            tmrBall.Interval = 30;
            tmrBall.Tick += new EventHandler(tmrBall_Tick);
            tmrBall.Start();


            tmrGoalScored.Interval = 1000;
            tmrGoalScored.Tick += new EventHandler(tmrGoalScored_Tick);


            tmrEnergyLoss.Interval = 1000;
            tmrEnergyLoss.Tick += new EventHandler(tmrEnergyGain_Tick);



           
            


            vfnInitializePlayers();




            // P1 oyuncularýný listeye ekle
            lstPlayersLeft.Add(pbxGKLeft);
            lstPlayersLeft.Add(pbxLBLeft);
            lstPlayersLeft.Add(pbxRBLeft);
            lstPlayersLeft.Add(pbxDMFLeft);
            lstPlayersLeft.Add(pbxLFLeft);
            lstPlayersLeft.Add(pbxRFLeft);
            // P1 oyuncularý kütüphanesi
            dctPbxVSPlayer.Add(pbxGKLeft, pLeftGK);
            dctPbxVSPlayer.Add(pbxLBLeft, pLeftLB);
            dctPbxVSPlayer.Add(pbxRBLeft, pLeftRB);
            dctPbxVSPlayer.Add(pbxDMFLeft, pLeftDMF);
            dctPbxVSPlayer.Add(pbxLFLeft, pLeftLF);
            dctPbxVSPlayer.Add(pbxRFLeft, pLeftRF);

   
            // P2 oyuncularýný listeye ekle
            lstPlayersRight.Add(pbxGKRight);
            lstPlayersRight.Add(pbxLBRight);
            lstPlayersRight.Add(pbxRBRight);
            lstPlayersRight.Add(pbxDMFRight);
            lstPlayersRight.Add(pbxLFRight);
            lstPlayersRight.Add(pbxRFRight);
            // P2 oyuncularý kütüphanesi
            dctPbxVSPlayer.Add(pbxGKRight, pRightGK);
            dctPbxVSPlayer.Add(pbxLBRight, pRightLB);
            dctPbxVSPlayer.Add(pbxRBRight, pRightRB);
            dctPbxVSPlayer.Add(pbxDMFRight, pRightDMF);
            dctPbxVSPlayer.Add(pbxLFRight, pRightLF);
            dctPbxVSPlayer.Add(pbxRFRight, pRightRF);

           


            foreach (PictureBox var in lstPlayersLeft)
            {
                lstOrgListOfPlayersLeft.Add(var.Location);
            }


            foreach (PictureBox var in lstPlayersRight)
            {
                lstOrgListOfPlayersRight.Add(var.Location);
            }


            // Picturebox eventleri
            foreach (Control cnt in this.Controls)
            {
                if (cnt is PictureBox)
                {
   
                    PictureBox pbx = cnt as PictureBox;

                    
                    pbx.MouseMove += new MouseEventHandler(pbx_MouseMove);
                    pbx.MouseLeave += new EventHandler(pbx_MouseLeave);
                    pbx.MouseClick += new MouseEventHandler(pbx_MouseClick);
                }
            }

            

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrEnergyGain_Tick(object sender, EventArgs e)
        {

               // Player Left
            foreach (PictureBox pbx in lstPlayersLeft)
            {
                CPlayer playerL = dctPbxVSPlayer[pbx];
                // Enerji güncelleniyor
                // Enerji her durumda düþüyor
                if (playerL.Energy < 100)
                    playerL.Energy++;
            }

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbx_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pbxSender = sender as PictureBox;
            CPlayer whoIsThatPlayer = dctPbxVSPlayer[pbxSender];


            // Önce oyunu bi durdur
            tmrGame.Stop();
            tmrBall.Stop();

            // Bilgi ekranýný aç
            F_Info fInfo = new F_Info();
            fInfo.fillPlayerInfo(whoIsThatPlayer);
            fInfo.ShowDialog();

            // Oyunu tekrar aç
            tmrGame.Start();
            tmrBall.Start();

        }


        /// <summary>
        /// 
        /// </summary>
        private void vfnInitializePlayers()
        {
            /*
            // LEFT GK
            pLeftGK.Position = EPosition.GK;
            pLeftGK.Priority = EPriority.Pass;
            pLeftGK.Speed = ESpeed.Fast;
            pLeftGK.Power = EPower.High;

            pLeftGK.Energy = 100;
            pLeftGK.nOfDribble = 0;
            pLeftGK.nOfPass = 0;
            pLeftGK.nOfShoot = 0;
            pLeftGK.blnAlways = false;


            // LEFT LB
            pLeftLB.Position = EPosition.LB;
            pLeftLB.Priority = EPriority.Pass;
            pLeftLB.Speed = ESpeed.Fast;
            pLeftLB.Power = EPower.Normal;

            pLeftLB.Energy = 100;
            pLeftLB.nOfDribble = 0;
            pLeftLB.nOfPass = 0;
            pLeftLB.nOfShoot = 0;
            pLeftLB.blnAlways = false;


            // LEFT RB
            pLeftRB.Position = EPosition.RB;
            pLeftRB.Priority = EPriority.Pass;
            pLeftRB.Speed = ESpeed.Low;
            pLeftRB.Power = EPower.Normal;

            pLeftRB.Energy = 100;
            pLeftRB.nOfDribble = 0;
            pLeftRB.nOfPass = 0;
            pLeftRB.nOfShoot = 0;
            pLeftRB.blnAlways = false;


            // LEFT DMF
            pLeftDMF.Position = EPosition.DMF;
            pLeftDMF.Priority = EPriority.Dribbling;
            pLeftDMF.Speed = ESpeed.Normal;
            pLeftDMF.Power = EPower.Normal;

            pLeftDMF.Energy = 100;
            pLeftDMF.nOfDribble = 0;
            pLeftDMF.nOfPass = 0;
            pLeftDMF.nOfShoot = 0;
            pLeftDMF.blnAlways = false;


            // LEFT LF
            pLeftLF.Position = EPosition.LF;
            pLeftLF.Priority = EPriority.Shoot;
            pLeftLF.Speed = ESpeed.Normal;
            pLeftLF.Power = EPower.Normal;

            pLeftLF.Energy = 100;
            pLeftLF.nOfDribble = 0;
            pLeftLF.nOfPass = 0;
            pLeftLF.nOfShoot = 0;
            pLeftLF.blnAlways = false;

            // LEFT RF
            pLeftRF.Position = EPosition.RF;
            pLeftRF.Priority = EPriority.Shoot;
            pLeftRF.Speed = ESpeed.Normal;
            pLeftRF.Power = EPower.Normal;

            pLeftRF.Energy = 100;
            pLeftRF.nOfDribble = 0;
            pLeftRF.nOfPass = 0;
            pLeftRF.nOfShoot = 0;
            pLeftRF.blnAlways = false;

            */

            // --------------------------
            // Right Player

            // Right GK
            pRightGK.Position = EPosition.GK;
            pRightGK.Priority = EPriority.Pass;
            pRightGK.Speed = ESpeed.Fast;
            pRightGK.Power = EPower.High;

            pRightGK.Energy = 100;
            pRightGK.nOfDribble = 0;
            pRightGK.nOfPass = 0;
            pRightGK.nOfShoot = 0;
            pRightGK.blnAlways = false;


            // Right LB
            pRightLB.Position = EPosition.LB;
            pRightLB.Priority = EPriority.Pass;
            pRightLB.Speed = ESpeed.Fast;
            pRightLB.Power = EPower.Normal;

            pRightLB.Energy = 100;
            pRightLB.nOfDribble = 0;
            pRightLB.nOfPass = 0;
            pRightLB.nOfShoot = 0;
            pRightLB.blnAlways = false;


            // Right RB
            pRightRB.Position = EPosition.RB;
            pRightRB.Priority = EPriority.Pass;
            pRightRB.Speed = ESpeed.Low;
            pRightRB.Power = EPower.Normal;

            pRightRB.Energy = 100;
            pRightRB.nOfDribble = 0;
            pRightRB.nOfPass = 0;
            pRightRB.nOfShoot = 0;
            pRightRB.blnAlways = false;


            // Right DMF
            pRightDMF.Position = EPosition.DMF;
            pRightDMF.Priority = EPriority.Dribbling;
            pRightDMF.Speed = ESpeed.Normal;
            pRightDMF.Power = EPower.Normal;

            pRightDMF.Energy = 100;
            pRightDMF.nOfDribble = 0;
            pRightDMF.nOfPass = 0;
            pRightDMF.nOfShoot = 0;
            pRightDMF.blnAlways = false;


            // Right LF
            pRightLF.Position = EPosition.LF;
            pRightLF.Priority = EPriority.Shoot;
            pRightLF.Speed = ESpeed.Normal;
            pRightLF.Power = EPower.Normal;

            pRightLF.Energy = 100;
            pRightLF.nOfDribble = 0;
            pRightLF.nOfPass = 0;
            pRightLF.nOfShoot = 0;
            pRightLF.blnAlways = false;

            // Right RF
            pRightRF.Position = EPosition.RF;
            pRightRF.Priority = EPriority.Shoot;
            pRightRF.Speed = ESpeed.Normal;
            pRightRF.Power = EPower.Normal;

            pRightRF.Energy = 100;
            pRightRF.nOfDribble = 0;
            pRightRF.nOfPass = 0;
            pRightRF.nOfShoot = 0;
            pRightRF.blnAlways = false;

            
        }


        /// <summary>
        /// Gol olduðunda çalýþan timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrGoalScored_Tick(object sender, EventArgs e)
        {
            
            iValTmrGoalScored++;
           
          
            if (iValTmrGoalScored == 2)
            {

                for (int i = 0; i < lstPlayersLeft.Count; i++)
                {
                    lstPlayersLeft[i].Location = new Point(lstOrgListOfPlayersLeft[i].X, lstOrgListOfPlayersLeft[i].Y);
                }

                for (int i = 0; i < lstPlayersRight.Count; i++)
                {
                    lstPlayersRight[i].Location = new Point(lstOrgListOfPlayersRight[i].X, lstOrgListOfPlayersRight[i].Y);
                }

                pbxBall.Location = new Point(this.Width / 2 - 10, this.Height / 2 - 10);
                
                iValTmrGoalScored = 0;
                lblGoal.Visible = false;
                FSK = int.Parse(numericUpDown1.Value.ToString());
                
                tmrGoalScored.Stop();
                tmrGame.Start();
            }
            
        }


   


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrBall_Tick(object sender, EventArgs e)
        {
            iValTmrBall++;


            // Sürtünme kuvveti (FSK) = 10
            if (iValTmrBall > 0 && iValTmrBall % FSK == 0)
            {

                if (ballDevX < 0)
                    ballDevX += 1;
                else if (ballDevX > 0)
                    ballDevX -= 1;
                else
                    ballDevX = 0;

                if (ballDevY < 0)
                    ballDevY += 1;
                else if (ballDevY > 0)
                    ballDevY -= 1;
                else
                    ballDevY = 0;
                 
            }

            updateBallLoc(ballDevX , ballDevY);
        }


        /// <summary>
        /// OYUN TÝMER FONKSÝYONU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrGame_Tick(object sender, EventArgs e)
        {

            iValTmrEnergyLoss++;


            Random rnd = new Random();

            // Topa en yakýn oyuncuyu bul ve topa yönlendir
            vfnFindTheClosestPlayer(pbxBall.Location);
            
            // Tüm oyuncular için default yönelme durumu rastgele olarak ayarlanýyor
            foreach (Control cnt in this.Controls)
            {
                if (cnt is PictureBox)
                {
                    
                    PictureBox pbx = cnt as PictureBox;
                    
                    int off_x = rnd.Next(0, 2);
                    int off_y = rnd.Next(0, 2);  

                    // Top haricindeki tüm picturebox bileþenleri futbolcudur
                    if (pbx.Name != pbxBall.Name)
                    {
                       
                        // Topa yönlenme vektörü
                        int devX = pbxBall.Location.X - pbx.Location.X;
                        int devY = pbxBall.Location.Y - pbx.Location.Y;


                        // Kaleciler kaleye yakýn konumlansýn
                        //if (pbx.Name == pbxGKLeft.Name)
                        //{
                        //    devX = (GoalLeft.Bounds.Right) - pbx.Location.X;
                        //    devY = (GoalLeft.Location.Y + GoalLeft.Height / 2) - pbx.Location.Y;

                        //    off_x = 1;
                        //    off_y = 1;

                        //}
                        //// Kaleciler kaleye yakýn konumlansýn
                        //if (pbx.Name == pbxGKRight.Name)
                        //{
                        //    devX = (GoalRight.Bounds.Left - pbx.Width) - pbx.Location.X;
                        //    devY = (GoalRight.Location.Y + GoalRight.Height / 2) - pbx.Location.Y;

                        //    off_x = 1;
                        //    off_y = 1;

                        //}
                       
                        // Topa yakýn olmayan tüm futbolcular topa doðru rand(0,2) kadar yaklaþýyor
                        pbx.Location = new Point(pbx.Location.X + Math.Sign(devX) * off_x, pbx.Location.Y + Math.Sign(devY) * off_y);
                        
                        // Konumlar güncelleniyor
                        vfnUpdateList(pbx);

                    }
                }
            }



            // Hamle için rasgele deðiþken
            Random rndAct = new Random();
            int iRndAct = 0;  

            // ----------------- PLAYER LEFT ---------------------------------
            //
            #region PLAYER LEFT TIMER EVENTS
            foreach (PictureBox pbx in lstPlayersLeft)
            {

                // Top P1(Left) oyuncularýnda ise
                if (pbx.Bounds.IntersectsWith(pbxBall.Bounds) && blnLeftHasTheBall == false)
                {
                    blnLeftHasTheBall = true;


                    // 0-20 arasýnda rastgele sayý seçiliyor
                    // Öncelik: Shoot ise 
                    // 0-10 arasý Shoot
                    // 10-15 arasý Pass
                    // 15-20 arasý Dribbling
                    // always: checked ise 0-20 arasý Shoot

                    bool rangeShoot = false, rangePass = false, rangeDribbling = false;
                    CPlayer selectedPlayer = dctPbxVSPlayer[pbx];
                    selectedPlayerAtThisMoment = selectedPlayer;
                    iRndAct = rndAct.Next(0, 20);



                    // Eðer kullanýcýnýn oyuncusuna top geldiyse
                    if (pbxUser != null)
                    {
                        if (pbx.Name == pbxUser.Name)
                        {
                            if (blnUserPass)
                            {
                                selectedPlayer.blnAlways = true;
                                selectedPlayer.Priority = EPriority.Pass;
                            }
                            else if (blnUserShoot)
                            {
                                selectedPlayer.blnAlways = true;
                                selectedPlayer.Priority = EPriority.Shoot;
                            }
                            else if (blnUserDribble)
                            {
                                selectedPlayer.blnAlways = true;
                                selectedPlayer.Priority = EPriority.Dribbling;
                            }

                        }
                    }


                    // Önceliðe göre aralýklar belirleniyor
                    switch (selectedPlayer.Priority)
                    {
                        case EPriority.Dribbling:
                            rangeDribbling = (iRndAct < 11);
                            rangePass = (iRndAct >= 11) && (iRndAct < 16);
                            rangeShoot = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangeDribbling = true;
                                rangePass = rangeShoot = false;
                            }

                            break;
                        case EPriority.Pass:
                            rangePass = (iRndAct < 11);
                            rangeDribbling = (iRndAct >= 11) && (iRndAct < 16);
                            rangeShoot = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangePass = true;
                                rangeDribbling = rangeShoot = false;
                            }
                            break;
                        case EPriority.Shoot:
                            rangeShoot = (iRndAct < 11);
                            rangePass = (iRndAct >= 11) && (iRndAct < 16);
                            rangeDribbling = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangeShoot = true;
                                rangeDribbling = rangePass = false;
                            }
                            break;
                        default:
                            break;
                    }



                    // kaleci ile arasýnda rakip yoksa þut çek
                    int nOfRightPlayer = 0, nOfPlayerInTriangle = 0;
                    foreach (PictureBox var in lstPlayersRight)
                    {
                        if (pbx.Bounds.Left > var.Bounds.Left)
                            nOfRightPlayer++;

                        if (PointInTriangle(var.Location, pbx.Location, GoalRight.Location, new Point(GoalRight.Location.X, GoalRight.Location.Y + GoalRight.Height)))
                            nOfPlayerInTriangle++;
                    }


                    // Geride en az 5 kiþi kalmýþsa VEYA
                    // Kaleyi gördüðü noktada en fazla bir kiþi varsa top sür ya da þut çek
                    if (nOfRightPlayer >= 5 || nOfPlayerInTriangle <= 1)
                    {
                        if (selectedPlayer.Priority != EPriority.Dribbling)
                            rangeShoot = true;
                        else
                            rangeDribbling = true;
                    }


                    // SHOOT
                    if (rangeShoot)
                    {
                        pbxBall.Location = new Point(pbx.Location.X + pbx.Width, pbx.Location.Y);

                        ballDevX = Math.Sign(GoalRight.Location.X - pbx.Location.X) * 7 * (int)selectedPlayer.Power;
                        ballDevY = Math.Sign((GoalRight.Location.Y + GoalRight.Height / 2) - pbx.Location.Y) * rnd.Next(1, 7) * (int)selectedPlayer.Power;

                        selectedPlayer.nOfShoot++;
                    }
                    // PASS
                    else if (rangePass)
                    {


                        // kendi haricinde rasgele bir takým arkadaþý seçiliyor
                        int rndIdx = rndAct.Next(lstPlayersLeft.Count);
                        if (lstPlayersLeft[rndIdx].Name == pbx.Name)
                        {
                            rndIdx = (rndIdx + 1) % lstPlayersLeft.Count;
                        }

                        int unitVectorX = Math.Sign(lstPlayersLeft[rndIdx].Location.X - pbx.Location.X);
                        int unitVectorY = Math.Sign(lstPlayersLeft[rndIdx].Location.Y - pbx.Location.Y);

                        pbxBall.Location = new Point(pbx.Location.X + pbx.Width * (unitVectorX), pbx.Location.Y + pbx.Height * (unitVectorY));

                        ballDevX = unitVectorX * 4 * (int)selectedPlayer.Power;
                        ballDevY = unitVectorY * 4 * (int)selectedPlayer.Power;

                        selectedPlayer.nOfPass++;


                    }
                    // DRIBBLING
                    else if (rangeDribbling)
                    {
                        int unitVectorX = Math.Sign(GoalRight.Location.X - pbx.Location.X);
                        int unitVectorY = Math.Sign(GoalRight.Location.Y - pbx.Location.Y);

                        pbxBall.Location = new Point(pbx.Location.X + pbx.Width * (unitVectorX), pbx.Location.Y + pbx.Height * (unitVectorY));

                        // Dribbling
                        ballDevX = unitVectorX * rndAct.Next(3, 7);
                        ballDevY = unitVectorY * rndAct.Next(2, 5);

                        selectedPlayer.nOfDribble++;

                        // Belli bir mesafe katedilince þut çek
                        if (pbx.Bounds.Right > this.Width * 4 / 5)
                        {
                            pbxBall.Location = new Point(pbx.Location.X + pbx.Width, pbx.Location.Y);

                            ballDevX = Math.Sign(GoalRight.Location.X - pbx.Location.X) * 7 * (int)selectedPlayer.Power;
                            ballDevY = Math.Sign((GoalRight.Location.Y + GoalRight.Height / 2) - pbx.Location.Y) * rnd.Next(1, 7) * (int)selectedPlayer.Power;

                            selectedPlayer.nOfShoot++;
                        }


                    }

                }
                else
                {
                    blnLeftHasTheBall = false;
                }


            } 
            #endregion


            // ----------------- PLAYER RIGHT ---------------------------------
            //

            
            #region PLAYER RIGHT TIMER EVENTS
            foreach (PictureBox pbx in lstPlayersRight)
            {
                if (pbx.Bounds.IntersectsWith(pbxBall.Bounds) && blnRightHasTheBall == false)
                {
                    blnRightHasTheBall = true;


                    // 0-20 arasýnda rastgele sayý seçiliyor
                    // Öncelik: Shoot ise 
                    // 0-10 arasý Shoot
                    // 10-15 arasý Pass
                    // 15-20 arasý Dribbling
                    // always: checked ise 0-20 arasý Shoot

                    bool rangeShoot = false, rangePass = false, rangeDribbling = false;
                    CPlayer selectedPlayer = dctPbxVSPlayer[pbx];
                    selectedPlayerAtThisMoment = selectedPlayer;
                    iRndAct = rndAct.Next(0, 20);

                    switch (selectedPlayer.Priority)
                    {
                        case EPriority.Dribbling:
                            rangeDribbling = (iRndAct < 11);
                            rangePass = (iRndAct >= 11) && (iRndAct < 16);
                            rangeShoot = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangeDribbling = true;
                                rangePass = rangeShoot = false;
                            }

                            break;
                        case EPriority.Pass:
                            rangePass = (iRndAct < 11);
                            rangeDribbling = (iRndAct >= 11) && (iRndAct < 16);
                            rangeShoot = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangePass = true;
                                rangeDribbling = rangeShoot = false;
                            }
                            break;
                        case EPriority.Shoot:
                            rangeShoot = (iRndAct < 11);
                            rangePass = (iRndAct >= 11) && (iRndAct < 16);
                            rangeDribbling = (iRndAct >= 16);

                            if (selectedPlayer.blnAlways)
                            {
                                rangeShoot = true;
                                rangeDribbling = rangePass = false;
                            }
                            break;
                        default:
                            break;
                    }


                    // kaleci ile arasýnda rakip yoksa þut çek
                    int nOfLeftPlayer = 0, nOfPlayerInTriangle = 0;
                    foreach (PictureBox var in lstPlayersLeft)
                    {
                        if (pbx.Bounds.Left < var.Bounds.Left)
                            nOfLeftPlayer++;

                        if (PointInTriangle(var.Location, pbx.Location, GoalLeft.Location, new Point(GoalLeft.Location.X, GoalLeft.Location.Y + GoalRight.Height)))
                            nOfPlayerInTriangle++;
                    }

                    //Console.WriteLine("nOfLeftPlayer:" + nOfLeftPlayer + "\t PlayerInTriangle: " + nOfPlayerInTriangle);

                    // Geride en az 5 kiþi kalmýþsa þut çek
                    if (nOfLeftPlayer >= 5)
                    {
                        rangeDribbling = true;
                        rangePass = rangeShoot = false;
                    }

                    // Kaleyi gördüðü noktada en fazla bir kiþi varsa þut çek
                    if (nOfPlayerInTriangle <= 1)
                        rangeShoot = true;


                    // SHOOT
                    if (rangeShoot)
                    {
                        pbxBall.Location = new Point(pbx.Location.X - pbx.Width, pbx.Location.Y);

                        ballDevX = Math.Sign(GoalLeft.Location.X - pbx.Location.X) * 7 * (int)selectedPlayer.Power;
                        ballDevY = Math.Sign((GoalLeft.Location.Y + GoalLeft.Height / 2) - pbx.Location.Y) * rnd.Next(1, 7) * (int)selectedPlayer.Power;


                        selectedPlayer.nOfShoot++;
                    }

                    // PASS
                    else if (rangePass)
                    {

                        int rndIdx = rndAct.Next(lstPlayersRight.Count);
                        if (lstPlayersRight[rndIdx].Name == pbx.Name)
                        {
                            rndIdx = (rndIdx + 1) % lstPlayersRight.Count;
                        }

                        int unitVectorX = Math.Sign(lstPlayersRight[rndIdx].Location.X - pbx.Location.X);
                        int unitVectorY = Math.Sign(lstPlayersRight[rndIdx].Location.Y - pbx.Location.Y);

                        pbxBall.Location = new Point(pbx.Location.X + pbx.Width * (unitVectorX), pbx.Location.Y + pbx.Height * (unitVectorY));

                        ballDevX = unitVectorX * 4 * (int)selectedPlayer.Power;
                        ballDevY = unitVectorY * 4 * (int)selectedPlayer.Power;

                        selectedPlayer.nOfPass++;

                    }

                        // DRIBBLING
                    else if (rangeDribbling)
                    {
                        int unitVectorX = Math.Sign(GoalLeft.Location.X - pbx.Location.X);
                        int unitVectorY = Math.Sign(GoalLeft.Location.Y - pbx.Location.Y);

                        pbxBall.Location = new Point(pbx.Location.X + pbx.Width * (unitVectorX), pbx.Location.Y + pbx.Height * (unitVectorY));

                        // Dribbling
                        ballDevX = unitVectorX * rndAct.Next(3, 7);
                        ballDevY = unitVectorY * rndAct.Next(2, 5);

                        selectedPlayer.nOfDribble++;

                        // Belli bir mesafe katedilince þut çek
                        if (pbx.Bounds.Left < this.Width * 1 / 5)
                        {
                            pbxBall.Location = new Point(pbx.Location.X - pbx.Width, pbx.Location.Y);

                            ballDevX = Math.Sign(GoalLeft.Location.X - pbx.Location.X) * 7 * (int)selectedPlayer.Power;
                            ballDevY = Math.Sign((GoalLeft.Location.Y + GoalLeft.Height / 2) - pbx.Location.Y) * rnd.Next(1, 7) * (int)selectedPlayer.Power;

                            selectedPlayer.nOfShoot++;
                        }


                    }
                }
                else
                {
                    blnRightHasTheBall = false;
                }
            }
            #endregion
            

           // Direkten dönme durumu
            if (pbxBall.Bounds.IntersectsWith(lblLeftPost1.Bounds))
                lblLeftPost1.BackColor = Color.White;
            else
                lblLeftPost1.BackColor = Color.ForestGreen;

            if (pbxBall.Bounds.IntersectsWith(lblLeftPost2.Bounds))
                lblLeftPost2.BackColor = Color.White;
            else
                lblLeftPost2.BackColor = Color.ForestGreen;


            if (pbxBall.Bounds.IntersectsWith(lblRightPost1.Bounds))
                lblRightPost1.BackColor = Color.White;
            else
                lblRightPost1.BackColor = Color.SteelBlue;

            if (pbxBall.Bounds.IntersectsWith(lblRightPost2.Bounds))
                lblRightPost2.BackColor = Color.White;
            else
                lblRightPost2.BackColor = Color.SteelBlue;




            // Gol olma durumu (Right Scored)
            //if (pbxBall.Bounds.IntersectsWith(GoalLeft.Bounds))
            //if (pbxBall.Bounds.Right < GoalLeft.Bounds.Left)
            if (pbxBall.Bounds.Right < lblLeftBound.Bounds.Left)
            {
                blnGoalScored = true;
                FSK = 2;

                //ballDevX = 0;
                //ballDevY = 0;
                //pbxBall.Location = new Point(GoalLeft.Bounds.Right + 1, pbxBall.Location.Y);
                //pbxBall.Location = new Point(this.Width/2, this.Height/2);

                //GoalLeft.BackColor = Color.Red;

                rightScored++;
                selectedPlayerAtThisMoment.nOfGoal++;

                lblScoreLeft.Text = leftScored.ToString();
                lblScoreRight.Text = rightScored.ToString();
                lblGoal.ForeColor = Color.SteelBlue;
                lblGoal.Visible = true;

                tmrGame.Stop();
                tmrGoalScored.Start();
            }
            else
            {
                GoalLeft.BackColor = Color.Transparent;
            }

            // Gol olma durumu (Left Scored)
            //if (pbxBall.Bounds.IntersectsWith(GoalRight.Bounds))
            //if (pbxBall.Bounds.Left > GoalRight.Bounds.Right)
            if (pbxBall.Bounds.Left > lblRightBound.Bounds.Right)
            {
                blnGoalScored = true;
                FSK = 2;

                //ballDevX = 0;
                //ballDevY = 0;
                //pbxBall.Location = new Point(GoalRight.Bounds.Left - pbxBall.Width - 1, pbxBall.Location.Y);
                

                //GoalRight.BackColor = Color.Red;

                leftScored++;
                selectedPlayerAtThisMoment.nOfGoal++;

                lblScoreLeft.Text = leftScored.ToString();
                lblScoreRight.Text = rightScored.ToString();
                lblGoal.ForeColor = Color.ForestGreen;
                lblGoal.Visible = true;

                tmrGame.Stop();
                tmrGoalScored.Start();
            }
            else
            {
                GoalRight.BackColor = Color.Transparent;
            }



            this.Update();


        }


        float sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbx_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbx_MouseMove(object sender, MouseEventArgs e)
        {

            this.Cursor = Cursors.Hand;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pbx"></param>
        private void vfnUpdateList(PictureBox pbx)
        {
            for (int i = 0; i < lstPlayersLeft.Count; i++)
            {
                if (lstPlayersLeft[i].Name == pbx.Name)
                {
                    lstPlayersLeft[i] = pbx;
                }
            }

            for (int i = 0; i < lstPlayersRight.Count; i++)
            {
                if (lstPlayersRight[i].Name == pbx.Name)
                {
                    lstPlayersRight[i] = pbx;
                }
            }
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private void vfnFindTheClosestPlayer(Point ptBall)
        {
            int minLeft = int.MaxValue, minRight = int.MaxValue, minIdxLeft = 0, minIdxRight = 0;
           

            // Left
            for (int i = 0; i < lstPlayersLeft.Count; i++)
            {
                int dist = (ptBall.X - lstPlayersLeft[i].Location.X) * (ptBall.X - lstPlayersLeft[i].Location.X) +
                    (ptBall.Y - lstPlayersLeft[i].Location.Y) * (ptBall.Y - lstPlayersLeft[i].Location.Y);

                if (dist < minLeft)
                {
                    minLeft = dist;
                    minIdxLeft = i;
                }
            }

            // Right
            for (int i = 0; i < lstPlayersLeft.Count; i++)
            {
                int dist = (ptBall.X - lstPlayersRight[i].Location.X) * (ptBall.X - lstPlayersRight[i].Location.X) +
                    (ptBall.Y - lstPlayersRight[i].Location.Y) * (ptBall.Y - lstPlayersRight[i].Location.Y);

                if (dist < minRight)
                {
                    minRight = dist;
                    minIdxRight = i;
                }
            }


            // Calculate for LEFT
            int devX = ptBall.X - lstPlayersLeft[minIdxLeft].Location.X;
            int devY = ptBall.Y - lstPlayersLeft[minIdxLeft].Location.Y;

            CPlayer selectedPlayer = dctPbxVSPlayer[lstPlayersLeft[minIdxLeft]];
            int speedVal = (int)selectedPlayer.Speed + 1;
            
            lstPlayersLeft[minIdxLeft].Location = new Point(lstPlayersLeft[minIdxLeft].Location.X + Math.Sign(devX) * speedVal, lstPlayersLeft[minIdxLeft].Location.Y + Math.Sign(devY) * speedVal);
            selectedPlayer.TotalDistance += (ulong)((Math.Abs(devX) +  Math.Abs(devY)) / 100);


            // Enerji güncelleniyor
            // Enerji bu durumda hýz oranýnda düþüyor
            int energyLoss = (int)selectedPlayer.TotalDistance / 1000;
            if (selectedPlayer.Energy > energyLoss)
                selectedPlayer.Energy = 100 - energyLoss;

            selectedPlayer.Power = (EPower)(int)(selectedPlayer.Energy / 34 + 1);
        

            if (selectedPlayer.TotalDistance == ulong.MaxValue)
                selectedPlayer.TotalDistance = 0;
                    

            // Calculate for RIGHT
            devX = ptBall.X - lstPlayersRight[minIdxRight].Location.X;
            devY = ptBall.Y - lstPlayersRight[minIdxRight].Location.Y;

            selectedPlayer = dctPbxVSPlayer[lstPlayersRight[minIdxRight]];
            speedVal = (int)selectedPlayer.Speed + 1;

            lstPlayersRight[minIdxRight].Location = new Point(lstPlayersRight[minIdxRight].Location.X + Math.Sign(devX) * speedVal, lstPlayersRight[minIdxRight].Location.Y + Math.Sign(devY) * speedVal);
            selectedPlayer.TotalDistance += (ulong)((Math.Abs(devX) + Math.Abs(devY)) / 100);

            if (selectedPlayer.TotalDistance == ulong.MaxValue)
                selectedPlayer.TotalDistance = 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void updateBallLoc(int x, int y)
        {
            pbxBall.Location = new Point(pbxBall.Location.X + x, pbxBall.Location.Y + y);
        }

       



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxBall_LocationChanged(object sender, EventArgs e)
        {


            if (pbxBall.Location.X > lblRightBound.Bounds.Right + pbxBall.Width 
                /*&&(pbxBall.Bounds.Bottom < lblRightGoalBoundDown.Bounds.Top && pbxBall.Bounds.Top > lblRightGoalBoundUp.Bounds.Bottom)*/)
            {

                // Kale içi üst sýnýr yansýmasý
                if (pbxBall.Bounds.Top < lblRightGoalBoundUp.Bounds.Bottom)
                {
                    pbxBall.Location = new Point(pbxBall.Location.X, lblRightGoalBoundUp.Bounds.Bottom);
                    ballDevY *= -1;
                    ballDevY -= 1;
                }

                // Kale içi alt sýnýr yansýmasý
                else if (pbxBall.Bounds.Bottom > lblRightGoalBoundDown.Bounds.Top)
                {
                    pbxBall.Location = new Point(pbxBall.Location.X, lblRightGoalBoundDown.Bounds.Top - pbxBall.Height);
                    ballDevY *= -1;
                    ballDevY += 1;
                }
            }


            if (pbxBall.Location.X < lblLeftBound.Bounds.Right - pbxBall.Width)
            {

                // Kale içi Üst sýnýr yansýmasý
                if (pbxBall.Bounds.Top < lblLeftGoalBoundUp.Bounds.Bottom)
                {
                    pbxBall.Location = new Point(pbxBall.Location.X, lblLeftGoalBoundUp.Bounds.Bottom);
                    ballDevY *= -1;
                    ballDevY -= 1;
                }

                // Alt sýnýr yansýmasý
                if (pbxBall.Bounds.Bottom > lblLeftGoalBoundDown.Bounds.Top)
                {
                    pbxBall.Location = new Point(pbxBall.Location.X, lblLeftGoalBoundDown.Bounds.Top - pbxBall.Height);
                    ballDevY *= -1;
                    ballDevY += 1;
                }
            }
       



            // Top en alt sýnýrda
            if (pbxBall.Location.Y > lblLowerBound.Location.Y - pbxBall.Height)
            {
                pbxBall.Location = new Point(pbxBall.Location.X, lblLowerBound.Location.Y - pbxBall.Height);
                ballDevY *= -1;
                ballDevY += 1;

            }
                // Top en üst sýnýrda
            else if (pbxBall.Location.Y < lblUpperBound.Location.Y)
            {
                pbxBall.Location = new Point(pbxBall.Location.X, lblUpperBound.Location.Y);
                ballDevY *= -1;
                ballDevY -= 1;
            }
                // Top en sað sýnýrda
            else if (pbxBall.Location.X > lblRightBound.Location.X - pbxBall.Width )
            {

                // Sað taraf kaleye gol olmadýðý sürece top yansýr
                if (pbxBall.Bounds.Bottom > lblRightGoalBoundDown.Bounds.Top || pbxBall.Bounds.Top < lblRightGoalBoundUp.Bounds.Bottom)
                {

                    pbxBall.Location = new Point(lblRightBound.Location.X - pbxBall.Width, pbxBall.Location.Y);
                    ballDevX *= -1;
                    ballDevX += 1;
                }
                // Gol durumunda kalenin içinde yansýmalar gerçekleþir
                else
                {

                  

                    // Kale içi sað sýnýr yansýmasý
                    if (pbxBall.Location.X > lblRightGoalBoundRight.Bounds.Left - pbxBall.Width)
                    {
                        pbxBall.Location = new Point(lblRightGoalBoundRight.Bounds.Left - pbxBall.Width, pbxBall.Location.Y);
                        ballDevX *= -1;
                        ballDevX += 1;
                    }
                    //// Kale içi sol sýnýr yansýmasý
                    //else if (pbxBall.Location.X < lblRightBound.Bounds.Right)
                    //{
                    //    pbxBall.Location = new Point(lblRightBound.Bounds.Right, pbxBall.Location.Y);
                    //    ballDevX *= -1;
                    //    ballDevX += 1;
                    //}
                }
            }
                // Top en sol sýnýrda
            else if (pbxBall.Location.X < lblLeftBound.Bounds.Right)
            {
                // Sol taraf kaleye gol olmadýðý sürece top yansýr
                if (pbxBall.Bounds.Bottom > lblLeftGoalBoundDown.Bounds.Top || pbxBall.Bounds.Top < lblLeftGoalBoundUp.Bounds.Bottom)
                {
                    pbxBall.Location = new Point(lblLeftBound.Bounds.Right, pbxBall.Location.Y);
                    ballDevX *= -1;
                    ballDevX -= 1;
                }
                else
                {
                    // Kale içi sol sýnýr yansýmasý
                    if (pbxBall.Location.X < lblLeftGoalBoundLeft.Bounds.Right)
                    {
                        pbxBall.Location = new Point(lblLeftGoalBoundLeft.Bounds.Right, pbxBall.Location.Y);
                        ballDevX *= -1;
                        ballDevX -= 1;
                    } 
                        // Kale içi sað sýnýr yansýmasý
                    //if (pbxBall.Location.X > lblLeftBound.Bounds.Left)
                    //{
                    //    pbxBall.Location = new Point(lblLeftBound.Bounds.Left, pbxBall.Location.Y);
                    //    ballDevX *= -1;
                    //    ballDevX += 1;
                    //}

                   
 
                }
            }
           
            


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                tmrBall.Stop();
                tmrGame.Stop();
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                tmrBall.Start();
                tmrGame.Start();
            }
        }


        public void setGK(CPlayer p)
        {
            pLeftGK = p;
        }

        public void setLB(CPlayer p)
        {
            pLeftLB = p;
        }

        public void setRB(CPlayer p)
        {
            pLeftRB = p;
        }

        public void setDMF(CPlayer p)
        {
            pLeftDMF = p;
        }

        public void setLF(CPlayer p)
        {
            pLeftLF = p;
        }

        public void setRF(CPlayer p)
        {
            pLeftRF = p;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private bool blnUserDribble = false;
        private bool blnUserShoot = false;
        private bool blnUserPass = false;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Main_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Q)
            {

                int minLeft = int.MaxValue, minIdxLeft = 0;
                Point ptBall = pbxBall.Location;

                // Left
                for (int i = 0; i < lstPlayersLeft.Count; i++)
                {
                    int dist = (ptBall.X - lstPlayersLeft[i].Location.X) * (ptBall.X - lstPlayersLeft[i].Location.X) +
                        (ptBall.Y - lstPlayersLeft[i].Location.Y) * (ptBall.Y - lstPlayersLeft[i].Location.Y);

                    if (dist < minLeft)
                    {
                        minLeft = dist;
                        minIdxLeft = i;
                    }
                }



                pbxUser = lstPlayersLeft[minIdxLeft];
                foreach (PictureBox var in lstPlayersLeft)
                {
                    var.BackColor = Color.Transparent;
                }
                pbxUser.BackColor = Color.Black;

            }
            else if (e.KeyData == Keys.S)
            {

                blnUserPass = true;
                blnUserDribble = blnUserShoot = false;

            }
            else if (e.KeyData == Keys.D)
            {

                blnUserShoot = true;
                blnUserDribble = blnUserPass = false;

            }
            else if (e.KeyData == Keys.W)
            {

                blnUserDribble = true;
                blnUserShoot = blnUserPass = false;

            }
            
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FSK = int.Parse(numericUpDown1.Value.ToString());
        }


    }
}