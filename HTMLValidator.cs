/*
 * This program validate the html tags and based on that it shows the appropriate message
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4b
{
    public partial class Form1 : Form
    {
        Stack<string> htmlStartingTags = new Stack<string>(); // Stores all the starting tags

        // contains a list of self closing tags
        List<string> selfClosingTags = new List<string> { "<area>","<base>","<br>", "<embed>", "<hr>",
                                                        "<iframe>", "<img>", "<input>", "<link>", "<meta>",
                                                        "<param>", "<source>", "<track>" };

        String line = "";  // Stores whole line

        int linenumber = 0;  // Stores line number

        string fileName = ""; // stores fille name

        String selectedFile; // stores whole path of the file

        public Form1()
        {
            InitializeComponent();

            messageLabel.Text = "No File Loaded"; // Sets the default message 

            checkTagsToolStripMenuItem.Enabled = false; // disable the check tags item 

        }

        // Open the default open menu which only allows to open html files
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
            OpenFileDialog openFile = new OpenFileDialog(); // Open the open file dialog box

            openFile.Filter = "HTML-Files(*.htm;*.html)|*.htm;*.html;"; // filter only only files

            if (openFile.ShowDialog() == DialogResult.OK) 
            {
                checkTagsToolStripMenuItem.Enabled = true; // enable the check tags item

                htmlTagsListBox.Items.Clear(); // CLear the list box

                selectedFile = openFile.FileName; // if file is selected then it stores the whole path into this variable

                fileName = openFile.SafeFileName; // Stores only file name with it's extension

                messageLabel.Text = fileName + " is loaded." ; // stores the file name into label

            }
        }

        // Validate the selected html file
        private void CheckTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            htmlTagsListBox.Items.Clear(); // CLear the list box
            {
                // try - catch block to handle errors
                try
                {
                    StreamReader fileInput = new StreamReader(selectedFile); // Reads the selected html file

                    // While loop will run until it reaches the end of the file
                    while (!fileInput.EndOfStream)
                    {
                        line = fileInput.ReadLine(); // Stores the line

                        line = line.ToLower(); // convert alll the characters into lower case

                        string htmlTag = ""; // stores the single tag

                        string foundTag = "notYet"; // flag to check for starting or closing tag

                        for (int i = 0; i < line.Length; i++) // loop through single line
                        {
                            if (line[i] == '<') // if the character starts with < then it sets the flag to openTag
                            {
                                foundTag = "openTag";

                                htmlTag += line[i]; // adds the character into string
                            }

                            else if (foundTag == "openTag" || foundTag == "closingTag") // check the flag conditions
                            {
                                htmlTag += line[i];

                                if (line[i] == '/') // check for the closing tag
                                {
                                    if (htmlTag[1] == '/')
                                        foundTag = "closingTag"; // sets the flag to closingTag
                                    else
                                        foundTag = "openTag"; // sets the flag to openTag
                                }

                                else if (line[i] == '>') // check if the tag is at the end or not
                                {
                                    if ((foundTag == "openTag"))
                                    {
                                        if (htmlTag.Contains("=") || htmlTag.Contains("!")) // sets the tage to open tag if they are input or doctype tag
                                        {
                                            int position = htmlTag.IndexOf(" "); // stores the position of blank space

                                            htmlTag = htmlTag.Substring(0, position) + ">"; // remove the space from that and add closing angular bracket
                                        }
                                        htmlTagsListBox.Items.Add("Found Opening Tag : " + htmlTag); // add the html tag into listbox

                                        htmlTagsListBox.Items.Add(" ");

                                        if (!selfClosingTags.Contains(htmlTag)) // condition to check for the self closing tags
                                        {
                                            htmlTag = htmlTag.Insert(1, "/"); // will create all tage into closing tag i.e. <html> become </html>

                                            htmlStartingTags.Push(htmlTag); // store the tag in stack 
                                        }
                                    }
                                    else if ((foundTag == "closingTag") && (htmlTag[1] == '/')) // condition for closing tag
                                    {
                                        if (htmlStartingTags.Peek() == htmlTag) // compare the closing tag with the top inserted tag
                                        {
                                            htmlStartingTags.Pop(); // remove the tag if it found the closing tag
                                        }

                                        else if (htmlStartingTags.Peek() != htmlTag)
                                        {
                                            fileInput.ReadToEnd(); // sets the fileinput to the end

                                            break; // break the loop
                                        }
                                        htmlTagsListBox.Items.Add("Found Closing Tag : " + htmlTag); // add the closing tag into listbox

                                        htmlTagsListBox.Items.Add(" ");
                                    }

                                    htmlTag = ""; // set the htmlTag to null

                                    foundTag = "endOfTag"; // set the flag to enOfTag
                                }
                            }
                        }

                        linenumber++; // increment the line number by one
                    }
                    fileInput.Close(); // close the html file

                    // print the message based on the count of the stack
                    if (htmlStartingTags.Count == 0)
                    {
                        messageLabel.Text = "Your file " + fileName + " have all the valid tags";
                    }
                    else
                    {
                        messageLabel.Text = "Your file " + fileName + " missing following tag: " + htmlStartingTags.Peek();
                    }
                }

                catch (FileNotFoundException ex)  // will check file not found exception
                {
                    Console.WriteLine("File not found.\n error is: " + ex.Message);
                }


                catch (FormatException ex) // will check data format exception
                {
                    Console.WriteLine($"Error on line {linenumber + 1} reading line {line} - {ex.Message} ");
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close(); // close the form
        }

    }
}
