using FBNews.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBNews
{
    public partial class Form1 : Form
    {
        string token = "EAACEdEose0cBACZAmyTu5EXmZASXlOAGmGz0x28m02RSCFg7Nd8EKOrqZA2pXPguIjogs1hJGxKN4FBNEIVK1jDzLjFKkVMVQ5RwXqyWrs10b05rHfhwjSwND2qIpqa2NARzA6S2HTgSfUt5CKAHJ00tzVpnpiYdovkTaOVixioLarig4ggM5LlXxBeHZCsBfRBLuHeDjrV8ZCnNwaXe5";
       // string Url = "https://graph.facebook.com/155869377766434/posts?limit=100&access_token=";
        List<page> pages;
        FBNewsDbEntities fbEntities;
        string[] keywords;
        string path;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int postcount = 0;
            fbEntities = new FBNewsDbEntities();
            label6.Text = "Loading";

          //  Url = Url + token;
            pages = fbEntities.pages.ToList();
            keywords = fbEntities.keywords.Select(c => c.keywords).ToArray();


            foreach (page page in pages)
            {



                path = "https://graph.facebook.com/" + page.pagefbid + "/posts?limit=100&access_token=" + token;
                var clientRepsonse = "";
                try
                {
                    using (var client = new WebClient())
                    {
                        clientRepsonse = client.DownloadString(path);
                    }
                    int count = 0;

                    dynamic deserialize_post = JsonConvert.DeserializeObject(clientRepsonse);
                    if (deserialize_post.data != null)
                    {

                        // check if posts exist
                        var listresponse = deserialize_post.data;
                        DateTime lastCheckDate = listresponse[0].created_time;

                        for (int i = 0; i < listresponse.Count; i++)
                        {
                            count = listresponse.Count;
                            string message = listresponse[i].message;
                            DateTime createdate = listresponse[i].created_time;
                            string id = listresponse[i].id;



                            if (!string.IsNullOrEmpty(message))
                            {
                                foreach (string keyword in keywords)
                                {
                                    if (message.ToLower().Contains(keyword.ToLower()) && !(fbEntities.FBNews.Any(c=>c.post_id==id)))
                                    {
                                        FBNew fb = new FBNew();
                                        fb.created_at = createdate;
                                        fb.message = message;
                                        fb.pagename = page.pagename;
                                        fb.post_id = id;
                                        fbEntities.FBNews.Add(fb);
                                        fbEntities.SaveChanges();
                                        postcount++;

                                    }
                                }

                            }
                        }
                        page.lastcheckdate = lastCheckDate;
                        fbEntities.SaveChanges();
                    }
                   // MessageBox.Show(count + "");

                    // Do further processing.
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "");
                }
            }

            MessageBox.Show("Done ! We Found  " +postcount+ "  new posts");
            label6.Text = "";



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string key = "";
            keyword keyword = new keyword();
            if (textBox4.Text == null)
            {
                MessageBox.Show("Please Empty values not allowed");
                return;
            }

            key = textBox4.Text;
            keyword.keywords = key;

            if (fbEntities.keywords.Any(c => c.keywords.Trim().ToLower() == key.ToLower().Trim()))
            {
                MessageBox.Show("This Keyword already exist !");
                return;

            }

            fbEntities.keywords.Add(keyword);
            if (fbEntities.SaveChanges() == 1)
                MessageBox.Show("Added Sucessfully ");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string pageid = "";
            string pagename = "";

            page page = new page();

            if (string.IsNullOrWhiteSpace(textBox3.Text) ||string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please Empty values not allowed");
                return;
            }
            if (fbEntities.pages.Any(c => c.pagefbid.Trim().ToLower() == textBox3.Text.ToLower().Trim()))
            {
                MessageBox.Show("This Page already exist !");
                return;

            }
            pageid = textBox3.Text;
            pagename = textBox5.Text;
            page.pagefbid = pageid;
            page.pagename=pagename;


            fbEntities.pages.Add(page);
            if (fbEntities.SaveChanges() == 1)
                MessageBox.Show("Added Sucessfully ");



        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


    }
}
