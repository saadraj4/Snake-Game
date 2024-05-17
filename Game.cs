
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;
namespace Snake
{
	
    //Use Of Classes and Creation of Objects
	public class Game
	{
        public int highscore = 1080;
        public Game(int height,int width)
		{
			game_width = width;
			game_height = height;
			
			snake_area_x = new Logic();
			snake_area_y = new Logic();
			
			wall_area_x= new Logic();
			wall_area_y= new Logic();
			
			snake_skin_color = Brushes.DarkOliveGreen;
			snake_body_color = Brushes.GreenYellow;
			back_color = Brushes.Black;
			
			wall_skin_color = Brushes.BurlyWood;
			wall_body_color = Brushes.Peru;
			
			bait_bool=false;
			bait_skin_color = Brushes.DarkRed;
			bait_body_color = Brushes.OrangeRed;
			
			node_length = 20;
			node_border_length =2;
			
			bait.X = -1;
			bait.Y = -1;
			
			snake_route = 2;
			snake_route_temp=2;
			point =0;
          
			level_point = 500;

			myfont = new PrivateFontCollection();
            myfont.AddFontFile(Application.StartupPath + "\\fonts\\Demonized.ttf");
			
			
			fontfamily = myfont.Families[0];
			
			game_status = "menu_1";
			game_level = 1;
			max_level = 6;
			
			speed_control = "medium";
			snake_speed = new int[3] {150,100,50};
		}
		
		public PrivateFontCollection myfont;
		public FontFamily fontfamily;

		                                                                                                                                  public SoundPlayer sound_eated;
		                                                                                                                                public SoundPlayer sound_menu;
		                                                                                                                                    public SoundPlayer sound_gameover;
		
		public int game_width;
		public int game_height;
        //Use Of Classes and Creation of Objects
        public Logic snake_area_x;
		public Logic snake_area_y;
		public Logic wall_area_x;
		public Logic wall_area_y;
		
		public Brush snake_skin_color;
		public Brush snake_body_color;
		public Brush back_color;
        //Use Of Classes and Creation of Objects
        public Brush wall_skin_color;
		public Brush wall_body_color;
		
		public Point bait = new Point();
		public bool bait_bool;
		public bool eated;
		public Brush bait_skin_color;
		public Brush bait_body_color;
        //Use Of Classes and Creation of Objects
        public int node_x;
		public int node_y;
		public int node_length;
		public int node_border_length;
		
		
		public int snake_length;
		public int snake_route;
		public int snake_route_temp;
		public int point;
        
        public int level_point;
		public int wait_time;
		
		public Bitmap bmp;
		public Graphics graph;
		public Bitmap menu_bmp;
		public Graphics menu_graph;
		
		public int key_control;
		public string game_status;
		public int game_level;
		public int max_level;
		public string sound_control;
		public string speed_control;
		public int[] snake_speed;
		
		public void Load_Graphic() 
		{
			bmp = new Bitmap(game_width,game_height);
			graph = Graphics.FromImage(bmp);

           
            graph.TextRenderingHint = TextRenderingHint.AntiAlias ;

			graph.FillRectangle(back_color,0,0,game_width,game_height);
		}
        //Abstraction
        public void Add_Snake_Node(int x,int y) 
		{
			graph.FillRectangle(snake_skin_color,x*node_length,y*node_length,node_length,node_length);
			graph.FillRectangle(snake_body_color,x*node_length,y*node_length,node_length-node_border_length,node_length-node_border_length);
			
		}
        //Abstraction
        //Creation of Objects
        public void Delete_Snake_Node(int x, int y)  
		{
			graph.FillRectangle(back_color,x*node_length,y*node_length,node_length,node_length);
		}
		
		
		
		public void Move_Snake_Left()
		{
			node_x--;
		}
		
		public void Move_Snake_Right()
		{
			node_x++;
		}
		
		public void Move_Snake_Up()
		{
			node_y--;
		}
		
		public void Move_Snake_Down()
		{
			node_y++;
		}

        //Abstraction
        public bool Control_Bait_Coordinate()
		{
			Node temp_x = snake_area_x.first;
			Node temp_y = snake_area_y.first;
			
			for(int i= snake_area_x.length;i>0;i--)
			{
				if((bait.X == temp_x.value) && (bait.Y == temp_y.value))
					
					return true;
				
				else
				{
					temp_x = temp_x.next;
					temp_y = temp_y.next;
					
				}
			}
			
			temp_x = wall_area_x.first;
			temp_y = wall_area_y.first;
			
			for(int j= wall_area_x.length;j>0;j--)
			{
				if((bait.X == temp_x.value) && (bait.Y == temp_y.value))
					return true;
				else
				{
					temp_x = temp_x.next;
					temp_y = temp_y.next;
				}
			}
			
			return false;
		}
		
		
		
		public void Find_Coordinate_For_Bait()
		{
			Random rnd = new Random();
			bait.X = rnd.Next(1,game_width/node_length-1);
			
			bait.Y = rnd.Next(1,game_height/node_length-1);
			bait_bool = true;
			
		}
		
		
		
		public void Add_New_Bait(int x, int y)
		{
            
			if(!Control_Bait_Coordinate())
			{
				graph.FillRectangle(bait_skin_color,x*node_length,y*node_length,node_length,node_length);
				graph.FillRectangle(bait_body_color,x*node_length,y*node_length,node_length-node_border_length,node_length-node_border_length);
			}
			
			else bait_bool=false;
		}
		

		public bool Did_Snake_Eat_The_Bait()
		{
			if((node_x == bait.X) && (node_y==bait.Y))
				return true;
			else
				return false;
		}
		
		
		public bool Did_Snake_Crash_Byself(int temp_route)
		{
            
			int x = node_x*node_length;
			int y = node_y*node_length;
			try
			{
				if (temp_route == 4) {
					if (bmp.GetPixel(x + node_length/2, y + node_length/2) == bmp.GetPixel(x + node_length/2 - node_length, y + node_length/2))
					{
						return true;
					}
					
					if(bmp.GetPixel(x + node_length/2 - node_length, y + node_length/2) == new Pen(wall_body_color).Color)
					{
						return true;
					}
				}

				if (temp_route == 2) {
					if (bmp.GetPixel(x + node_length/2, y + node_length/2) == bmp.GetPixel(x + node_length/2 + node_length , y + node_length/2))
					{
						return true;
					}
					
					if(bmp.GetPixel(x + node_length/2 + node_length , y + node_length/2) == new Pen(wall_body_color).Color)
					{
						return true;
					}
				}

				if (temp_route == 1) {
					if (bmp.GetPixel(x + node_length/2, y + node_length/2) == bmp.GetPixel(x + node_length/2, y + node_length/2 - node_length))
					{
						return true;
					}
					
					if(bmp.GetPixel(x + node_length/2, y + node_length/2 - node_length) == new Pen(wall_body_color).Color)
					{
						return true;
					}
				}

				if (temp_route == 3) {
					if (bmp.GetPixel(x + node_length/2, y + node_length/2) == bmp.GetPixel(x + node_length/2, y + node_length/2 + node_length))
					{
						return true;
					}
					
					if(bmp.GetPixel(x + node_length/2, y + node_length/2 + node_length) == new Pen(wall_body_color).Color)
					{
						return true;
					}
				}
				
			}
			catch (Exception e) { string str = e.Message.ToString(); return false; }

			return false;
		}
		
		
		public void Add_Wall_Node(int x1,int y1,int x2,int y2)
		{
			if(x1==x2)
			{
				while(y1<=y2)
				{
					graph.FillRectangle(wall_skin_color,x1*node_length,y1*node_length,node_length,node_length);
					graph.FillRectangle(wall_body_color,x1*node_length,y1*node_length,node_length-node_border_length,node_length-node_border_length);
					wall_area_x.Add_Node(x1);
					wall_area_y.Add_Node(y1);
					y1++;
				}
			}
			
			else if(y1==y2)
			{
				while(x1<=x2)
				{
					graph.FillRectangle(wall_skin_color,x1*node_length,y1*node_length,node_length,node_length);
					graph.FillRectangle(wall_body_color,x1*node_length,y1*node_length,node_length-node_border_length,node_length-node_border_length);
					wall_area_x.Add_Node(x1);
					wall_area_y.Add_Node(y1);
					x1++;
				}
			}
		}
		
		
		//STring Manipulations
		public void Main_Menu(int selected)
		{
			
			menu_bmp = new Bitmap(game_width,game_height);
			menu_graph = Graphics.FromImage(menu_bmp);

            
            menu_graph.TextRenderingHint = TextRenderingHint.AntiAlias;

			menu_graph.FillRectangle(back_color,0,0,game_width,game_height);
			
			
			Font arial3 = new Font( fontfamily, 70, FontStyle.Regular);
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
            
			menu_graph.DrawString("SNAKE", arial3,snake_skin_color,game_width/4,game_height/4);
			
            
			if(selected == 1)
			{
				menu_graph.DrawString(">START GAME", arial2,Brushes.FloralWhite,game_width/4 + 100,game_height/4 + 150);
				
				menu_graph.DrawString(" SETTINGS", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 190);
				menu_graph.DrawString(" HIGH SCORE", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 230);
				menu_graph.DrawString(" EXIT", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 270);
				
			}
			else if(selected == 2)
			{
                menu_graph.DrawString(" START GAME", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 150);
                
                menu_graph.DrawString(">SETTINGS", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString(" HIGH SCORE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString(" EXIT", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);

                selected = 3;
				
			}
			else if(selected == 3)
			{
				menu_graph.DrawString(" START GAME", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 150);
				
                menu_graph.DrawString(">SETTINGS", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString(" HIGH SCORE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString(" EXIT", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);
				
			}
			else if(selected == 4)
			{
				menu_graph.DrawString(" START GAME", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 150);
				
                menu_graph.DrawString(" SETTINGS", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 190);
				menu_graph.DrawString(">HIGH SCORE", arial2,Brushes.FloralWhite,game_width/4 + 100,game_height/4 + 230);
				menu_graph.DrawString(" EXIT", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 270);
				
			}
			else if(selected == 5)
			{
				menu_graph.DrawString(" START GAME", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 150);
				
				menu_graph.DrawString(" SETTINGS", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 190);
				menu_graph.DrawString(" HIGH SCORE", arial2,snake_body_color,game_width/4 + 100,game_height/4 + 230);
				menu_graph.DrawString(">EXIT", arial2,Brushes.FloralWhite,game_width/4 + 100,game_height/4 + 270);
			}

            DrawnSelect("SELECT");
			
			
		}
		

        
        private void DrawnBack()
        {
            Font arial = new Font(fontfamily, 20, FontStyle.Regular);
            menu_graph.DrawString("BACK", arial, Brushes.FloralWhite, 10, game_height - 70);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\back.bmp"), 130, game_height - 80);
        }

        
        private void DrawnSelect(string txt)
        {
            Font arial = new Font(fontfamily, 20, FontStyle.Regular);
            menu_graph.DrawString(txt, arial, Brushes.FloralWhite, game_width - 325, game_height - 70);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\select.bmp"), game_width - 160, game_height - 80);
        }


        //STring Manipulations
        public void About()
		{
			
			int x = game_width;
			int y = game_height;
			menu_bmp = new Bitmap(game_width,game_height);
			menu_graph = Graphics.FromImage(menu_bmp);

           
            menu_graph.TextRenderingHint = TextRenderingHint.AntiAlias ;

			menu_graph.FillRectangle(back_color,0,0,game_width,game_height);
			
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			Font arial3 = new Font( fontfamily, 70, FontStyle.Regular);
			
			menu_graph.DrawString("SNAKE", arial3,snake_skin_color,x/4,y/4);
			
			x= x/8 + x/10;
			y = y/2 -y/11;
			
			y += game_height/8;
			x -=x/5;

            if (highscore > point)
            {

                menu_graph.DrawString("HIGH SCORE\n " + highscore, arial2, Brushes.FloralWhite, x, y - 10);
            }
            DrawnBack();
		}
		
		
		public void Settings(int selected)
		{
			menu_bmp = new Bitmap(game_width,game_height);
			menu_graph = Graphics.FromImage(menu_bmp);

          
            menu_graph.TextRenderingHint = TextRenderingHint.AntiAlias ;

			menu_graph.FillRectangle(back_color,0,0,game_width,game_height);
			
			Font arial3 = new Font( fontfamily, 70, FontStyle.Regular);
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			menu_graph.DrawString("SNAKE", arial3,snake_skin_color,game_width/4,game_height/4);
			
            
			if(selected == 1) 
			{
				menu_graph.DrawString(">.  "  , arial2,Brushes.FloralWhite,game_width/4 + 50,game_height/4 + 150);
				menu_graph.DrawString(" >DISPLAY  " + game_width.ToString() +" : " + game_height.ToString(), arial2,snake_body_color,game_width/4 + 50,game_height/4 + 190);
				menu_graph.DrawString(" DIFFICULTY  "   + speed_control, arial2,snake_body_color,game_width/4 + 50,game_height/4 + 230);
			}
			
			if(selected == 2) 
			{
				menu_graph.DrawString("    " , arial2,snake_body_color,game_width/4 + 50,game_height/4 + 150);
				menu_graph.DrawString(">DISPLAY  " + game_width.ToString() +" : " + game_height.ToString(), arial2,Brushes.FloralWhite,game_width/4 + 50,game_height/4 + 190);
				menu_graph.DrawString("DIFFICULTY  "   + speed_control, arial2,snake_body_color,game_width/4 + 50,game_height/4 + 230);
			}
			
			if(selected == 3) 
			{
				menu_graph.DrawString("   "  , arial2,snake_body_color,game_width/4 + 50,game_height/4 + 150);
				menu_graph.DrawString(" DISPLAY  " + game_width.ToString() +" : " + game_height.ToString(), arial2,snake_body_color,game_width/4 + 50,game_height/4 + 190);
				menu_graph.DrawString("> DIFFICULTY  "   + speed_control, arial2,Brushes.FloralWhite,game_width/4 + 50,game_height/4 + 230);
			}

            DrawnSelect("CHANGE");
            DrawnBack();
		}
		
		
		public void Pause()
		{
			menu_bmp = new Bitmap(game_width,game_height);
			menu_graph = Graphics.FromImage(menu_bmp);

            
            menu_graph.TextRenderingHint = TextRenderingHint.AntiAlias ;

			menu_graph.FillRectangle(back_color,0,0,game_width,game_height);
			
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			
			menu_graph.DrawString("PAUSED", arial2	,snake_skin_color,game_width/2-50,game_height/2);
			
		}
		
		
		public void Clear_Level()
		{
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			
			graph.FillRectangle(bait_body_color, game_width/3,game_height/3,380,game_height/3);
			graph.DrawString("    CONGRATULATIONS\n       Level-"+game_level.ToString()+" CLEARED\n\nPRESS TO START LEVEL-"+(game_level+1).ToString(), arial2 ,Brushes.Thistle,game_width/3+25,game_height/3 + game_height/20);
			
		}
		
		
		public void Finish_Game()
		{
			Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			
			graph.FillRectangle(bait_body_color, game_width/3,game_height/3,380,game_height/3);
			graph.DrawString("    CONGRATULATIONS\n    YOU HAVE CLEARED\n         ALL LEVELS", arial2 ,Brushes.Thistle,game_width/3+25,game_height/3 + game_height/20);
			
		}
		

		
		public void Game_Over()
        {
            

            if (highscore < point)
            {
                highscore = point;
            }

           
            
            Font arial2 = new Font( fontfamily, 20, FontStyle.Regular);
			
			graph.FillRectangle(snake_skin_color, game_width/3-120,game_height/3,530,game_height/4);
            
			graph.DrawString("             GAME OVER\nPRESS ENTER TO CONTINUE", arial2 ,Brushes.Thistle,game_width/3-100,game_height/3 + game_height/20);
            
			
		}
	}
	
}
