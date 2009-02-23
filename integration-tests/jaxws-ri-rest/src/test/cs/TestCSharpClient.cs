namespace Jaxws.Ri.Rest {

  using NUnit.Framework;
  using System;
  using Org.Codehaus.Enunciate.Samples.Genealogy.Services;
  using Org.Codehaus.Enunciate.Samples.Genealogy.Cite;
  using Org.Codehaus.Enunciate.Samples.Genealogy.Data;
  using System.Web.Services.Protocols;
  using System.Collections;

  [TestFixture]
  public class FullAPITest {

    [Test]
    public void TestFullAPI() {
      SourceService sourceService = new SourceService();
      sourceService.Url = "http://localhost:8080/full/soap-services/sources/source";
      Source source = sourceService.GetSource("valid");
      Assert.IsNotNull(source);
      Assert.AreEqual("valid", source.Id);
      Assert.AreEqual("uri:some-uri", source.Link);
      Assert.AreEqual("some-title", source.Title);
      Assert.IsNull(sourceService.GetSource("invalid"));

      try {
        sourceService.GetSource("throw");
        Assert.Fail();
      }
      catch (SoapException) {
        //fall through...
      }

      try {
        sourceService.GetSource("unknown");
        Assert.Fail();
      }
      catch (SoapException) {
        //fall through...
      }

      Assert.AreEqual("newid", sourceService.AddInfoSet("somesource", new InfoSet()));
      Assert.AreEqual("okay", sourceService.AddInfoSet("othersource", new InfoSet()));
      Assert.AreEqual("intercepted", sourceService.AddInfoSet("SPECIAL", new InfoSet()));
      Assert.AreEqual("intercepted2", sourceService.AddInfoSet("SPECIAL2", new InfoSet()));
      Assert.AreEqual("resourceId", sourceService.AddInfoSet("resource", new InfoSet()));
      try {
        sourceService.AddInfoSet("unknown", new InfoSet());
        Assert.Fail("Should have thrown the exception.");
      }
      catch (SoapException) {
        //fall through...
      }

      PersonService personService = new PersonService();
      personService.Url = "http://localhost:8080/full/soap-services/PersonServiceService";
      ArrayList pids = new ArrayList();
      pids.Add("id1");
      pids.Add("id2");
      pids.Add("id3");
      pids.Add("id4");
      Person[] persons = personService.ReadPersons((string[])pids.ToArray(typeof(string)));
      Assert.AreEqual(4, persons.Length);
      foreach (Person pers in persons) {
        Assert.IsTrue(pids.Contains(pers.Id));
        Assert.IsNotNull(pers.Events);
        Assert.IsTrue(pers.Events.Length > 0);
        Assert.IsNotNull(pers.Events[0].Date);
        Assert.AreEqual(1970, pers.Events[0].Date.Year);
      }

      Person[] empty = personService.ReadPersons(null);
      Assert.IsTrue(empty == null || empty.Length == 0);

      personService.DeletePerson("somebody");
      try {
        personService.DeletePerson(null);
        Assert.Fail("Should have thrown the exception.");
      }
      catch (SoapException e) {
        //fall through...
      }

      try {
        personService.DeletePerson("SPECIAL");
        Assert.Fail("should have thrown an exception.");
      }
      catch (SoapException e) {
        //fall through...
      }

      Person person = new Person();
      person.Id = "new-person";
      Assert.AreEqual("new-person", personService.StorePerson(person).Id);

      System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
      byte[] pixBytes = encoding.GetBytes("this is a bunch of bytes that I would like to make sure are serialized correctly so that I can prove out that attachments are working properly");
      person.Picture = pixBytes;

      byte[] returnedPix = personService.StorePerson(person).Picture;
      Assert.AreEqual("this is a bunch of bytes that I would like to make sure are serialized correctly so that I can prove out that attachments are working properly", encoding.GetString(returnedPix));

      RelationshipService relationshipService = new RelationshipService();
      relationshipService.Url = "http://localhost:8080/full/soap-services/RelationshipServiceService";
      Relationship[] list = relationshipService.GetRelationships("someid");
      for (int i = 0; i < list.Length; i++) {
        Relationship relationship = list[i];
        Assert.AreEqual(i.ToString(), relationship.Id);
      }

      try {
        relationshipService.GetRelationships("throw");
        Assert.Fail("Should have thrown the relationship service exception, even though it wasn't annotated with @WebFault.");
      }
      catch (SoapException e) {
        //fall through
      }

      relationshipService.Touch();
    }
  }
}