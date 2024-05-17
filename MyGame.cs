
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading;
using Microsoft.DirectX.DirectInput;


using Microsoft.DirectX.DirectSound;

namespace Snake
{
	//Inheritance
	public partial class MyGame : Form
	{
        
       
        private BufferDescription bd;
        

		public MyGame()
		{
			InitializeComponent();
            

            bd = new BufferDescription();
            bd.ControlVolume = true;
            
		}
        //Inheritance
        //Object creation

        Game mygame = new Game(740,760);







        //Inheritance
        void MyGame_Form_Load(object sender, EventArgs e)
		{
			lb_point.Font = new Font( mygame.fontfamily, 12, FontStyle.Regular);
			lb_level.Font = new Font( mygame.fontfamily, 12, FontStyle.Regular);
			joysticks.InitDevices();
			Level_Design();
			Get_Ready();
			mygame.Main_Menu(1);
			pictureBox1.Image = mygame.menu_bmp;
            
			
		}
		
		
		void Get_Ready()
		{
			int i=mygame.snake_length;
			while( i> 0)
			{
				mygame.snake_area_x.Add_Node(mygame.node_x);
				mygame.snake_area_y.Add_Node(mygame.node_y);
				mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
				i--;
				mygame.node_x++;
			}
			mygame.node_x--;
		}
		
		
		void MyGame_TimerTick(object sender, EventArgs e)
		{
			Game_Playing();
		}
		
		
		public bool Game_Playing()
		{
			lb_level.Text = "LEVEL-" + mygame.game_level.ToString();
			lb_point.Text = mygame.point.ToString() + "/" + mygame.level_point.ToString();
			
            if(mygame.highscore < mygame.point)
            {
                mygame.highscore = mygame.point;
            }
			if(mygame.point >=  mygame.level_point)
			{
                
               
				Clear_Level();
				return true;
			}
			
            
			if(mygame.Did_Snake_Crash_Byself(mygame.snake_route))
			{
				MyGame_timer.Enabled = false;
				wait_timer.Enabled = true;
                Game_Over();
				return true;
				
			}
			
			
			if(mygame.snake_route == 1) mygame.Move_Snake_Up();
			else if(mygame.snake_route == 2) mygame.Move_Snake_Right();
			else if(mygame.snake_route == 3) mygame.Move_Snake_Down();
			else if(mygame.snake_route == 4) mygame.Move_Snake_Left();
			
			
			mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
			mygame.snake_area_x.Add_Node(mygame.node_x);
			mygame.snake_area_y.Add_Node(mygame.node_y);
			mygame.snake_route_temp= mygame.snake_route;
			
			
			
			if(!mygame.eated) {
				mygame.Delete_Snake_Node(mygame.snake_area_x.first.value,mygame.snake_area_y.first.value);
				mygame.snake_area_x.Delete_First_Node();
				mygame.snake_area_y.Delete_First_Node();
			}
			else mygame.eated = false;
			
			if(!mygame.bait_bool) {
				mygame.Find_Coordinate_For_Bait();
				if(!mygame.Control_Bait_Coordinate())
				{
					mygame.Add_New_Bait(mygame.bait.X,mygame.bait.Y);
					
				}
			}
			
			
			if(mygame.Did_Snake_Eat_The_Bait())
			{
                

				mygame.Find_Coordinate_For_Bait();
				
				if(!mygame.Control_Bait_Coordinate())
				{
					mygame.Add_New_Bait(mygame.bait.X,mygame.bait.Y);
					
				}
				
				else mygame.bait_bool=false;
				
				mygame.eated = true;
				mygame.point +=10;
				
			}
			
			pictureBox1.Image = mygame.bmp;
			pictureBox1.Refresh();
			
			
			
			if((mygame.node_x > (mygame.game_width/mygame.node_length-1)) && mygame.snake_route==2)
			{
				mygame.node_x = 0;
				mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
				mygame.snake_area_x.Add_Node(mygame.node_x);
				mygame.snake_area_y.Add_Node(mygame.node_y);
				if(!mygame.eated)
				{
					mygame.Delete_Snake_Node(mygame.snake_area_x.first.value,mygame.snake_area_y.first.value);
					mygame.snake_area_x.Delete_First_Node();
					mygame.snake_area_y.Delete_First_Node();
				}
				else mygame.eated = false;
				
			}
			else if(mygame.node_x < 0 && mygame.snake_route==4)
			{
				mygame.node_x = (mygame.game_width/mygame.node_length-1);
				mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
				mygame.snake_area_x.Add_Node(mygame.node_x);
				mygame.snake_area_y.Add_Node(mygame.node_y);
				if(!mygame.eated)
				{
					mygame.Delete_Snake_Node(mygame.snake_area_x.first.value,mygame.snake_area_y.first.value);
					mygame.snake_area_x.Delete_First_Node();
					mygame.snake_area_y.Delete_First_Node();
				}
				else mygame.eated = false;
			}
			if((mygame.node_y > (mygame.game_height/mygame.node_length-1)) && mygame.snake_route==3)
			{
				mygame.node_y = 0;
				mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
				mygame.snake_area_x.Add_Node(mygame.node_x);
				mygame.snake_area_y.Add_Node(mygame.node_y);
				if(!mygame.eated)
				{
					mygame.Delete_Snake_Node(mygame.snake_area_x.first.value,mygame.snake_area_y.first.value);
					mygame.snake_area_x.Delete_First_Node();
					mygame.snake_area_y.Delete_First_Node();
				}
				else mygame.eated = false;
			}
			else if(mygame.node_y < 0 && mygame.snake_route==1)
			{
				mygame.node_y = (mygame.game_height/mygame.node_length-1);
				mygame.Add_Snake_Node(mygame.node_x,mygame.node_y);
				mygame.snake_area_x.Add_Node(mygame.node_x);
				mygame.snake_area_y.Add_Node(mygame.node_y);
				if(!mygame.eated)
				{
					mygame.Delete_Snake_Node(mygame.snake_area_x.first.value,mygame.snake_area_y.first.value);
					mygame.snake_area_x.Delete_First_Node();
					mygame.snake_area_y.Delete_First_Node();
				}
				else mygame.eated = false;
				
			}
			return true;
			
		}
		
		
		//Encapsulations
		void MyGameKeyDown(object sender, KeyEventArgs e)
		{
			
			if(mygame.game_status == "menu_1" || mygame.game_status == "menu_2" || mygame.game_status == "menu_3" || mygame.game_status == "menu_4" ||
			   mygame.game_status == "menu_5" || mygame.game_status == "about" || mygame.game_status == "instructions" || mygame.game_status == "settings_1"
			   || mygame.game_status == "settings_2" || mygame.game_status == "settings_3" || mygame.game_status== "game_over" || mygame.game_status== "clear")
			{
				Menu_Control_For_Keyboard(e.KeyCode);
			}
			
			else if(mygame.game_status == "playing") 
			{
				if((e.KeyCode == Keys.Up) && (mygame.snake_route_temp!=3)) mygame.snake_route = 1;
				else if((e.KeyCode == Keys.Right) && (mygame.snake_route_temp!=4)) mygame.snake_route = 2;
				else if((e.KeyCode == Keys.Down) && (mygame.snake_route_temp!=1)) mygame.snake_route = 3;
				else if((e.KeyCode == Keys.Left) && (mygame.snake_route_temp!=2)) mygame.snake_route = 4;
				
				mygame.wait_time =20;
				MyGame_timer.Enabled = true;
				wait_timer.Enabled=false;
				if(e.KeyCode == Keys.P) Pause();

                
                if (e.KeyCode == Keys.Back)
                {
                    mygame.Game_Over();
                    MyGame_timer.Enabled = false;
                    wait_timer.Enabled = true;

                    Game_Create_Again();
                    lb_point.Text = "";
                    lb_level.Text = "MENU";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                }
                    
			}
			
			else if(mygame.game_status == "paused")
			{
				if(e.KeyCode == Keys.P)
					Pause();
            }
		}

        //Abstraction
        bool Menu_Control_For_Keyboard(Keys e)
		{
			
			if(mygame.game_status == "menu_1")
			{
				
				if(e == Keys.Enter)
				{
					
					mygame.game_status = "playing";
					mygame.snake_route = 2;
					MyGame_timer.Enabled = true;
					return true;
				}
				
				else if(e == Keys.Down)
				{

                    mygame.Main_Menu(3);            
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_3";  
					return true;
				}
			}
			
			else if(mygame.game_status == "menu_3")
			{
				if(e == Keys.Enter)
				{
					mygame.Settings(1);
					mygame.game_status = "settings_1";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				else if(e == Keys.Down)
				{
					
					mygame.Main_Menu(4);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_4";
					return true;
				}
				else if(e == Keys.Up)
				{

                    mygame.Main_Menu(1);            
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
                    mygame.game_status = "menu_1";  
					return true;
				}
			}
			
			
			else if(mygame.game_status == "menu_4")
			{
				if(e == Keys.Enter)
				{
					mygame.About();
					mygame.game_status = "about";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Down)
				{
					
					mygame.Main_Menu(5);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_5";
					return true;
				}
				
				else if(e == Keys.Up)
				{
					
					mygame.Main_Menu(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_3";
					return true;
				}
			}
			
			
			else if(mygame.game_status == "menu_5")
			{
				if(e == Keys.Enter)
				{
					Application.Exit();
				}
				
				else if(e == Keys.Up)
				{
					
					mygame.Main_Menu(4);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_4";
					return true;
				}
			}

			
			else if(mygame.game_status == "instructions")
			{
				if(e == Keys.Back)
				{
					mygame.Main_Menu(2);
					mygame.game_status = "menu_2";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_1")
			{
				if(e == Keys.Back)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Down)
				{
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}

                

                
                else if (e == Keys.Left)
                {
                    DecreaseVolume();
                    UpdateSettingSoundText();
                }

                
				else if(e == Keys.Enter)
				{
					
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_2")
			{
				if(e == Keys.Back)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Up)
				{
					mygame.game_status = "settings_1";
					mygame.Settings(1);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Down)
				{
					mygame.game_status = "settings_3";
					mygame.Settings(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Enter)
				{
					if(this.Size.Width < 791) this.Size = new Size(950,740);
					else if(this.Size.Width < 951) this.Size = new Size(1250,740);
					else if(this.Size.Width > 1249) this.Size = new Size(790,740);
					
					mygame.game_height = (this.Size.Height-50)-((this.Size.Height-50) % mygame.node_length);
					mygame.game_width = (this.Size.Width-50) - ((this.Size.Width-50) % mygame.node_length);
					
					Game_Create_Again();
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_3")
			{
				if(e == Keys.Back)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Up)
				{
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(e == Keys.Enter)
				{
					if(mygame.speed_control == "easy")
					{
						mygame.speed_control = "medium";
						Set_Game_Speed(1);
					}
					else if(mygame.speed_control == "medium")
					{
						mygame.speed_control = "hard";
						Set_Game_Speed(2);
					}
					else if(mygame.speed_control == "hard")
					{
						mygame.speed_control = "easy";
						Set_Game_Speed(0);
					}
					
					mygame.game_status = "settings_3";
					mygame.Settings(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "about")
			{
				if(e == Keys.Back)
				{
					mygame.Main_Menu(4);
					mygame.game_status = "menu_4";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			
			else if(mygame.game_status == "clear")
			{
				if(e == Keys.Enter)
				{
					Level_Design();
					Get_Ready();
					mygame.game_status = "playing";
					MyGame_timer.Enabled = true;
				}
			}
            //Abstraction

            else if (mygame.game_status == "game_over")
			{
				if(e == Keys.Enter)
				{
					Game_Create_Again();
					lb_point.Text = "";
					lb_level.Text = "MENU";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			//Abstraction
			Thread.Sleep(100);
			return true;
		}

        //STring Manipulations
        void Game_Over()
		{
            

			mygame.Game_Over();
			pictureBox1.Refresh();
			mygame.game_status = "game_over";

		}
		
		//Encapsulation
		void Joystick_timerTick(object sender, EventArgs e)
		{
			if(joysticks.joystick !=null)
			{
				joysticks.UpdateJoystick();
				
				if(mygame.game_status == "menu_1" || mygame.game_status == "menu_2" || mygame.game_status == "menu_3" || mygame.game_status == "menu_4" ||
				   mygame.game_status == "menu_5" || mygame.game_status == "about" || mygame.game_status == "instructions" || mygame.game_status == "settings_1"
				   || mygame.game_status == "settings_2" || mygame.game_status == "settings_3" || mygame.game_status== "game_over" || mygame.game_status== "clear")
					
				{
					Menu_Control();
					Thread.Sleep(100);
				}
				
				else if(mygame.game_status == "paused")
				{
					if(joysticks.state.GetButtons()[8] >= 128)
					{
						Pause();
						Thread.Sleep(100);
					}
				}
				
				else if(mygame.game_status == "playing")
				{
					if(joysticks.state.X != -40 || joysticks.state.Y != -40)
					{
						if(joysticks.state.X < -40 && mygame.snake_route_temp != 2 ) mygame.snake_route = 4;
						else if(joysticks.state.X > -40 && mygame.snake_route_temp != 4 ) mygame.snake_route = 2;
						
						if(joysticks.state.Y < -40 && mygame.snake_route_temp != 3 ) mygame.snake_route = 1;
						else if(joysticks.state.Y > -40 && mygame.snake_route_temp != 1 ) mygame.snake_route = 3;
						
						mygame.wait_time = 20;
						MyGame_timer.Enabled = true;
						wait_timer.Enabled=false;
						
					}
					
					else if(joysticks.state.GetButtons()[8] >= 128)
					{
						Pause();
						Thread.Sleep(100);
					}
					
				}
				
				
				
			}
		}
		
		
		public bool Menu_Control()
		{
			
			if(mygame.game_status == "menu_1")
			{
				
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					mygame.sound_menu.Stop();
					mygame.game_status = "playing";
					mygame.snake_route = 2;
					MyGame_timer.Enabled = true;
					return true;
				}
				
				if(joysticks.state.Y > -40)
				{
					
					mygame.Main_Menu(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_2";
				}
			}
			
		
			else if(mygame.game_status == "menu_2")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					
					mygame.game_status = "instructions";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				if(joysticks.state.Y > -40)
				{
					
					mygame.Main_Menu(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_3";
				}
				else if(joysticks.state.Y < -40)
				{
					
					mygame.Main_Menu(1);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_1";
				}
			}
			
		
			else if(mygame.game_status == "menu_3")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					mygame.Settings(1);
					mygame.game_status = "settings_1";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				if(joysticks.state.Y > -40)
				{
					
					mygame.Main_Menu(4);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_4";
				}
				else if(joysticks.state.Y < -40)
				{
					
					mygame.Main_Menu(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_2";
				}
			}
			
			
			else if(mygame.game_status == "menu_4")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					mygame.About();
					mygame.game_status = "about";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				if(joysticks.state.Y > -40)
				{
					
					mygame.Main_Menu(5);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_5";
				}
				
				else if(joysticks.state.Y < -40)
				{
					
					mygame.Main_Menu(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_3";
				}
			}
			
			
			else if(mygame.game_status == "menu_5")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					Application.Exit();
				}
				if(joysticks.state.Y < -40)
				{
					
					mygame.Main_Menu(4);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					mygame.game_status = "menu_4";
				}
			}
			
			else if(mygame.game_status == "instructions")
			{
				if(joysticks.state.GetButtons()[3] >= 128)
				{
					mygame.Main_Menu(2);
					mygame.game_status = "menu_2";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_1")
			{
				if(joysticks.state.GetButtons()[3] >= 128)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.Y > -40)
				{
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.GetButtons()[2] >= 128)
				{
					
					if(mygame.sound_control == "on") {
						mygame.sound_control="off";
						mygame.sound_menu.Stop();
					}
					else {
						mygame.sound_control = "on";
						mygame.sound_menu.PlayLooping();
					}
					
					mygame.Settings(1);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_2")
			{
				if(joysticks.state.GetButtons()[3] >= 128)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.Y < -40)
				{
					mygame.game_status = "settings_1";
					mygame.Settings(1);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.Y > -40)
				{
					mygame.game_status = "settings_3";
					mygame.Settings(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.GetButtons()[2] >= 128)
				{
					if(this.Size.Width < 791) this.Size = new Size(950,740);
					else if(this.Size.Width < 951) this.Size = new Size(1250,740);
					else if(this.Size.Width > 1249) this.Size = new Size(790,740);
					
					mygame.game_height = (this.Size.Height-50)-((this.Size.Height-50) % mygame.node_length);
					mygame.game_width = (this.Size.Width-50) - ((this.Size.Width-50) % mygame.node_length);
					
					Game_Create_Again();
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			else if(mygame.game_status == "settings_3")
			{
				if(joysticks.state.GetButtons()[3] >= 128)
				{
					mygame.Main_Menu(3);
					mygame.game_status = "menu_3";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.Y < -40)
				{
					mygame.game_status = "settings_2";
					mygame.Settings(2);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
				
				else if(joysticks.state.GetButtons()[2] >= 128)
				{
					if(mygame.speed_control == "easy")
					{
						mygame.speed_control = "hard";
						Set_Game_Speed(1);
					}
					else if(mygame.speed_control == "hard")
					{
						mygame.speed_control = "hardest";
						Set_Game_Speed(2);
					}
					else if(mygame.speed_control == "hardest")
					{
						mygame.speed_control = "easy";
						Set_Game_Speed(0);
					}
					
					mygame.game_status = "settings_3";
					mygame.Settings(3);
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
		
			else if(mygame.game_status == "about")
			{
				if(joysticks.state.GetButtons()[3] >= 128)
				{
					mygame.Main_Menu(4);
					mygame.game_status = "menu_4";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			
			
			else if(mygame.game_status == "clear")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					Level_Design();
					Get_Ready();
					mygame.game_status = "playing";
					MyGame_timer.Enabled = true;
				}
			}
			
			
			else if(mygame.game_status == "game_over")
			{
				if(joysticks.state.GetButtons()[2] >= 128)
				{
					Game_Create_Again();
					lb_point.Text = "";
					lb_level.Text = "MENU";
					pictureBox1.Image = mygame.menu_bmp;
					pictureBox1.Refresh();
					return true;
				}
			}
			
			
			return true;
		}
		
		void Wait_timerTick(object sender, EventArgs e)
		{
			mygame.wait_time--;
			if(mygame.wait_time==0)
			{
				wait_timer.Enabled = false;
				
			}
		}
		
		public void Level_Design()
		{
			
			int temp,temp2,y,i;
			
			mygame.Load_Graphic();
			mygame.node_x = 4;
			mygame.node_y = 4;
			mygame.snake_length = 7;
			
			
			if(mygame.game_level == 2)
			{
				
				temp = mygame.game_width/mygame.node_length -1;
				temp2 = mygame.game_height/mygame.node_length -1;
				mygame.Add_Wall_Node(0,0,temp,0);
				mygame.Add_Wall_Node(temp,0,temp,temp2);
				mygame.Add_Wall_Node(0,temp2,temp,temp2);
				mygame.Add_Wall_Node(0,0,0,temp2);
				
			}
		
			else if(mygame.game_level == 3)
			{
				int temp_x = (mygame.game_width/mygame.node_length)/4;
				int temp_y = (mygame.game_height/mygame.node_length)/4;
				
				mygame.Add_Wall_Node(temp_x,temp_y,temp_x+temp_x/2,temp_y);
				mygame.Add_Wall_Node(temp_x,temp_y,temp_x,temp_y+temp_y/2);
				
				mygame.Add_Wall_Node(temp_x,2*temp_y+temp_y/2+1,temp_x,3*temp_y);
				mygame.Add_Wall_Node(temp_x,3*temp_y,temp_x+temp_x/2,3*temp_y);
				
				mygame.Add_Wall_Node(2*temp_x+temp_x/2+1,3*temp_y,3*temp_x,3*temp_y);
				mygame.Add_Wall_Node(3*temp_x,3*temp_y-temp_y/2,3*temp_x,3*temp_y);
				
				mygame.Add_Wall_Node(2*temp_x+temp_x/2+1,temp_y,3*temp_x,temp_y);
				mygame.Add_Wall_Node(3*temp_x,temp_y,3*temp_x,temp_y+temp_y/2);
			}
			
			else if(mygame.game_level == 4)
			{
				
				temp = mygame.game_width/mygame.node_length -1;
				y = 2;
				i=5;
				
				while(i>0)
				{
					mygame.Add_Wall_Node(3,y,temp-3,y);
					y= y+10;
					i--;
				}
			}
			
			else if(mygame.game_level == 5)
			{
				temp = mygame.game_width/mygame.node_length -1;
				y = 6;
				i=3;
				
				while(i>0)
				{
					mygame.Add_Wall_Node(6,y,temp-6,y);
					y= y+10;
					i--;
				}
				y = 6;
				
				mygame.Add_Wall_Node(6,y,6,y+10);
				mygame.Add_Wall_Node(temp-6,y+10,temp-6,y+20);
				
				
				
			}
			
			else if(mygame.game_level == 6)
			{
				temp = mygame.game_width/mygame.node_length -1;
				y = 6;
				i=3;
				
				while(i>0)
				{
					mygame.Add_Wall_Node(6,y,temp-6,y);
					y= y+10;
					i--;
				}
				y = 6;
				
				mygame.Add_Wall_Node(6,y,6,y+10);
				mygame.Add_Wall_Node(temp-6,y+10,temp-6,y+20);
				
				
				temp = mygame.game_width/mygame.node_length -1;
				temp2 = mygame.game_height/mygame.node_length -1;
				mygame.Add_Wall_Node(0,0,temp,0);
				mygame.Add_Wall_Node(temp,0,temp,temp2);
				mygame.Add_Wall_Node(0,temp2,temp,temp2);
				mygame.Add_Wall_Node(0,0,0,temp2);
				
				
			}
			
		}
		
		public void Pause()
		{
			if(MyGame_timer.Enabled)
			{
				mygame.game_status = "paused";
				MyGame_timer.Enabled = false;
				mygame.Pause();
				pictureBox1.Image = mygame.menu_bmp;
				pictureBox1.Refresh();
			}
			
			else
			{
				mygame.game_status ="playing";
				MyGame_timer.Enabled = true;
			}
		}
        //Abstraction
        public void Clear_Level()
		{
			int level= mygame.game_level + 1;
			int game_height = mygame.game_height;
			int game_width = mygame.game_width;
			string sound = mygame.sound_control;
			MyGame_timer.Enabled = false;
			if(level > mygame.max_level)
			{
				level = 1;
				mygame.Finish_Game();
				pictureBox1.Refresh();
				mygame.game_status = "game_over";
			}
			else {
				mygame.Clear_Level();
				
				pictureBox1.Refresh();
				mygame = new Game(game_height,game_width);
				mygame.game_status = "clear";
				mygame.sound_control = sound;
				mygame.game_level = level;
			}
		}
		
        //Abstraction
		public void Set_Game_Speed(int i)
		{
			MyGame_timer.Interval = mygame.snake_speed[i];
		}
		
		
		void Game_Create_Again()
		{
			
			int h = mygame.game_height;
			int w = mygame.game_width;
			mygame = new Game(h,w);
			top_menu_panel.Size = new Size(w,top_menu_panel.Size.Height);
			MyGame_Form_Load(this,System.EventArgs.Empty);
		}

        
        private void IncreaseVolume()
        {
            
        }

       
        private void DecreaseVolume()
        {
            
        }
        
        private void UpdateVolume()
        {
           
        }

        
        private void UpdateSettingSoundText()
        {
            
            mygame.Settings(1);
            pictureBox1.Image = mygame.menu_bmp;
            pictureBox1.Refresh();
        }
	}
}