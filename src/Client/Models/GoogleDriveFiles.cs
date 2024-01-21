using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiDegree.Models
{

    public class DriveFileList
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public bool incompleteSearch { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public string webContentLink { get; set; }
        public string alternateLink { get; set; }
        public string embedLink { get; set; }
        public string iconLink { get; set; }
        public string thumbnailLink { get; set; }
        public string title { get; set; }
        public string mimeType { get; set; }
        public string description { get; set; }
        public Labels labels { get; set; }
        public bool copyRequiresWriterPermission { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public DateTime markedViewedByMeDate { get; set; }
        public string version { get; set; }
        public Parent[] parents { get; set; }
        public string downloadUrl { get; set; }
        public Userpermission userPermission { get; set; }
        public string originalFilename { get; set; }
        public string fileExtension { get; set; }
        public string md5Checksum { get; set; }
        public string fileSize { get; set; }
        public string quotaBytesUsed { get; set; }
        public string[] ownerNames { get; set; }
        public Owner[] owners { get; set; }
        public string lastModifyingUserName { get; set; }
        public Lastmodifyinguser lastModifyingUser { get; set; }
        public Capabilities capabilities { get; set; }
        public bool editable { get; set; }
        public bool copyable { get; set; }
        public bool writersCanShare { get; set; }
        public bool shared { get; set; }
        public bool explicitlyTrashed { get; set; }
        public bool appDataContents { get; set; }
        public string headRevisionId { get; set; }
        public Imagemediametadata imageMediaMetadata { get; set; }
        public string[] spaces { get; set; }
    }

    public class Labels
    {
        public bool starred { get; set; }
        public bool hidden { get; set; }
        public bool trashed { get; set; }
        public bool restricted { get; set; }
        public bool viewed { get; set; }
    }

    public class Userpermission
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public string selfLink { get; set; }
        public string role { get; set; }
        public string type { get; set; }
    }

    public class Lastmodifyinguser
    {
        public string kind { get; set; }
        public string displayName { get; set; }
        public Picture picture { get; set; }
        public bool isAuthenticatedUser { get; set; }
        public string permissionId { get; set; }
        public string emailAddress { get; set; }
    }

    public class Picture
    {
        public string url { get; set; }
    }

    public class Capabilities
    {
        public bool canCopy { get; set; }
        public bool canEdit { get; set; }
    }

    public class Imagemediametadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public int rotation { get; set; }
        public Location location { get; set; }
        public string date { get; set; }
        public string cameraMake { get; set; }
        public string cameraModel { get; set; }
        public float exposureTime { get; set; }
        public float aperture { get; set; }
        public bool flashUsed { get; set; }
        public float focalLength { get; set; }
        public int isoSpeed { get; set; }
        public string meteringMode { get; set; }
        public string exposureMode { get; set; }
        public string colorSpace { get; set; }
        public string whiteBalance { get; set; }
        public float exposureBias { get; set; }
        public float maxApertureValue { get; set; }
    }

    public class Location
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float altitude { get; set; }
    }

    public class Parent
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string selfLink { get; set; }
        public string parentLink { get; set; }
        public bool isRoot { get; set; }
    }

    public class Owner
    {
        public string kind { get; set; }
        public string displayName { get; set; }
        public Picture1 picture { get; set; }
        public bool isAuthenticatedUser { get; set; }
        public string permissionId { get; set; }
        public string emailAddress { get; set; }
    }

    public class Picture1
    {
        public string url { get; set; }
    }

}
