﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    [TestFixture]
    public class ArtistRepositoryFixture
    {
        private readonly ArtistRepository repository;

        public ArtistRepositoryFixture()
        {
            repository = new ArtistRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetAllArtistsReturnsExpectedValues()
        {
            var artists = await repository.GetAllArtists(CancellationToken.None);

            CollectionAssert.IsNotEmpty(artists);

            var firstArtist = artists.FirstOrDefault();

            Assert.That(firstArtist.ArtistId, Is.EqualTo(1));
            Assert.That(firstArtist.FirstName, Is.EqualTo("PENELOPE"));
            Assert.That(firstArtist.LastName, Is.EqualTo("GUINESS"));
        }
    }
}
