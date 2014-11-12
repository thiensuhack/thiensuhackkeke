// --------------------------------
// <copyright file="Metadata.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace EPubReader.Contents
{
    /// <summary>
    /// Information about the publication.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets the identifier of the publication.
        /// </summary>
        public Identifier Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the publication.
        /// </summary>
        public Title Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject collection of the publication.
        /// </summary>
        public Collection<Subject> Subjects
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description of the publication.
        /// </summary>
        public Description Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the creators of the publication.
        /// </summary>
        public Collection<Creator> Creators
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rights of the publication.
        /// </summary>
        public Collection<Rights> Rights
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the publisher of the publication.
        /// </summary>
        public Publisher Publisher
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the languages of the publication.
        /// </summary>
        public Collection<Language> Languages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dates of the publication.
        /// </summary>
        public Collection<Date> Dates
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the meta collection of the publication.
        /// </summary>
        public Collection<Meta> Metas
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Metadata class.
        /// </summary>
        public Metadata()
        {
        }

        internal Metadata(IEnumerable<XElement> metadataXElements)
        {
            // Identifier
            Identifier = new Identifier(metadataXElements);

            // Title
            Title = new Title(metadataXElements);

            // Subject
            Subjects = new Collection<Subject>();
            foreach (XElement metaXElement in metadataXElements.Elements(BasePurl.PurlOrgDcElementsNameSpace + "subject"))
            {
                Subjects.Add(new Subject(metaXElement));
            }

            // Description
            Description = new Description(metadataXElements);

            // Creator
            Creators = new Collection<Creator>();
            foreach (XElement metaXElement in metadataXElements.Elements(BasePurl.PurlOrgDcElementsNameSpace + "creator"))
            {
                Creators.Add(new Creator(metaXElement));
            }

            // Rights
            Rights = new Collection<Rights>();
            foreach (XElement metaXElement in metadataXElements.Elements(BasePurl.PurlOrgDcElementsNameSpace + "rights"))
            {
                Rights.Add(new Rights(metaXElement));
            }

            // Publisher
            Publisher = new Publisher(metadataXElements);

            // Languages
            Languages = new Collection<Language>();
            foreach (XElement metaXElement in metadataXElements.Elements(BasePurl.PurlOrgDcElementsNameSpace + "language"))
            {
                Languages.Add(new Language(metaXElement));
            }

            // Date
            Dates = new Collection<Date>();
            foreach (XElement metaXElement in metadataXElements.Elements(BasePurl.PurlOrgDcElementsNameSpace + "date"))
            {
                Dates.Add(new Date(metaXElement));
            }

            // Metas
            Metas = new Collection<Meta>();
            foreach (XElement metaXElement in metadataXElements.Elements(Loader.IdpfOrg2007OpfNameSpace + "meta"))
            {
                Metas.Add(new Meta(metaXElement));
            }
        }
    }
}
