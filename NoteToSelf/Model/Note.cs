using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteToSelf.Model
{
    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public List<string> Tags { get; set; }
        public bool IsNoteCategory { get; set; }

        public Note()
        {

        }

        public Note(string title, string description, DateTime created, DateTime lastModified, string tags)
        {
            Title = title;
            Description = description;
            Created = created;
            LastModified = lastModified;
            Tags = NoteHelper.GetTagsFromString(tags);
        }

        public Note(string title, string description, DateTime created, DateTime lastModified, List<string> tags)
        {
            Title = title;
            Description = description;
            Created = created;
            LastModified = lastModified;
            Tags = tags;
        }
    }

    public static class NoteHelper
    {
        public static List<string> GetTagsFromString(string tags)
        {
            if (tags == string.Empty)
                return null;

            return tags?.Split(',').Select(x => x.Trim()).Distinct().Where(x => !string.IsNullOrEmpty(x)).Select(x => x).ToList();
        }
    }
}
