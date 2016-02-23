### CExtensions-Patch

just a json patch .net library (in progress)

quick sample

[code]
              public async Task ShouldApplyPatch()
              {
                  dynamic originalDocument = JObject.Parse(@"
                                      {
                                       'baz': 'qux',
                                       'foo': 'bar'
                                       }"
                     );
      
                  string jsonDocument = (@"
                                      [
                                          { 'op': 'add', 'path': '$.boo', 'value': 'zoo' },
                                          { 'op': 'remove', 'path': '$.baz' },
                                          { 'op': 'copy', 'from':'$.foo','path': '$.qux' }
                                      ]"
                                      );
                  var doc = JsonPatchDocument.FromJson(jsonDocument);
      
                  await doc.ApplyTo(originalDocument);
      
                  string result = SerializeObject(originalDocument);
                  result.ShouldBe("{'foo':'bar','boo':'zoo','qux':'bar'}");
      
              }

[/code]