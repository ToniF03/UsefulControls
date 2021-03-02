using System.IO;
using TagLib.Utils;

namespace TagLib.ID3.ID3v1
{
    class ID3v1Tag
    {
        public string FileIdentifier;
        public string Title;
        public string Artist;
        public string Album;
        public string Year;
        public string Comment;
        public string Genre;
        public string TagVersion;

        public ID3v1Tag(FileInfo fileInfo)
        {
            FileStream mReader = fileInfo.OpenRead();
            byte[] mFileIdentifier = new byte[3];
            byte[] mTitle = new byte[30];
            byte[] mArtist = new byte[30];
            byte[] mAlbum = new byte[30];
            byte[] mYear = new byte[4];
            byte[] mComment = new byte[30];
            byte[] mGenre = new byte[1];
            mReader.Position = checked(mReader.Length - 128L);
            mReader.Read(mFileIdentifier, 0, 3);
            this.FileIdentifier = Mp3Utils.BytesToText(mFileIdentifier).Trim();
            if (this.FileIdentifier == "TAG")
            {
                this.TagVersion = "1.0";
                mReader.Read(mTitle, 0, 30);
                mReader.Read(mArtist, 0, 30);
                mReader.Read(mAlbum, 0, 30);
                mReader.Read(mYear, 0, 4);
                mReader.Read(mComment, 0, 30);
                mReader.Read(mGenre, 0, 1);
                this.Title = Mp3Utils.BytesToText(mTitle).Trim();
                this.Artist = Mp3Utils.BytesToText(mArtist).Trim();
                this.Album = Mp3Utils.BytesToText(mAlbum).Trim();
                this.Year = Mp3Utils.BytesToText(mYear).Trim();
                this.Comment = Mp3Utils.BytesToText(mComment).Trim();
                this.Genre = mGenre[0].ToString();
            }
            mReader.Close();
            mReader.Dispose();
        }
    }
}

