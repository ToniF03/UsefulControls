using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TagLib.ID3.ID3v1;
using TagLib.ID3.ID3v2;
using TagLib.ID3.Lib;

namespace TagLib.ID3
{
    class Mp3File
    {
        public FileInfo fileInfo;
        public MpegInfo mpegInfo;
        public ID3v1Tag Tag1;
        public ID3v2Tag Tag2;
        
        /// <summary>
        /// Retrieves the ID3 tag version.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string TagVersion
        {
            get
            {
                string val = "";
                if (this.Tag1.TagVersion != "")
                    val = this.Tag1.TagVersion;
                if (this.Tag2.TagVersion != "")
                    val = this.Tag2.TagVersion;
                return val;
            }
            set
            {
            }
        }

        public string Artist
        {
            get
            {
                string val = "";
                if (this.Tag1.Artist != "") val = this.Tag1.Artist;
                if (this.Tag2.Artist() != "") val = this.Tag2.Artist();
                return val;
            }
            set
            {
            }
        }

        public string Year
        {
            get
            {
                string val = "";
                if (this.Tag1.Year != "") val = this.Tag1.Year;
                if (this.Tag2.Year() != "") val = this.Tag2.Year();
                return val;
            }
            set
            {
            }
        }

        public string Title
        {
            get
            {
                string val = "";
                if (this.Tag1.Title != "") val = this.Tag1.Title;
                if (this.Tag2.Title() != "") val = this.Tag2.Title();
                return val;
            }
        }

        public string AlbumArtist
        {
            get
            {
                string val = "";
                if (this.Tag2.AlbumArtist() != "") val = this.Tag2.AlbumArtist();
                return val;
            }
            set
            {
            }
        }

        public string Album
        {
            get
            {
                string val = "";
                if (this.Tag1.Album != "") val = this.Tag1.Album;
                if (this.Tag2.Album() != "") val = this.Tag2.Album();
                return val;
            }
            set
            {
            }
        }

        public Image Cover
        {
            get
            {
                Image val = null;
                if (this.Tag2.Cover != null) val = this.Tag2.Cover;
                return val;
            }
            set { }
        }

        public string TrackNumber
        {
            get
            {
                string val = "";
                if (this.Tag2.TrackNumber() != "") val = this.Tag2.TrackNumber();
                return val;
            }
            set
            {
            }
        }

        public string Disc
        {
            get
            {
                string val = "";
                if (this.Tag2.Disc() != "") val = this.Tag2.Disc();
                return val;
            }
            set
            {
            }
        }

        public string Comment
        {
            get
            {
                string val = "";
                if (this.Tag1.Comment != "") val = this.Tag1.Comment;
                if (this.Tag2.Comment() != "") val = this.Tag2.Comment();
                return val;
            }
            set
            {
            }
        }

        public string Composer
        {
            get
            {
                string val = "";
                if (this.Tag2.Composer() != "") val = this.Tag2.Composer();
                return val;
            }
            set
            {
            }
        }

        public string Genre
        {
            get
            {
                string val = "";
                if (this.Tag1.Genre != "") val = this.Tag1.Genre;
                if (this.Tag2.Genre() != "") val = this.Tag2.Genre();
                return val;
            }
            set
            {
            }
        }

        public Mp3File(string argFilePath)
        {
            this.fileInfo = new System.IO.FileInfo(argFilePath);
            this.Tag1 = new ID3v1Tag(this.fileInfo);
            this.Tag2 = new ID3v2Tag(this.fileInfo);
            this.mpegInfo = new MpegInfo(this.fileInfo, Convert.ToInt32(this.Tag2.TagSize));
        }
    }
}
