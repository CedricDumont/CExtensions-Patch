### CExtensions-Patch

just a json patch .net library (in progress)

#### Usage example with Json as String


            var originalDocument = (@"
                                {
                                    'name':{'last_Name': 'Dumont', 'first_Name': 'Cedric'},
                                    'personal_adress_street':'heavenstreet',
                                    'personal_adress_nr':'10',
                                    'personal_adress_postCode':'4444',
                                    'personal_adress_City':'Paradise',
                                    'office_adress_street':'badStreet',
                                    'office_adress_nr':'666',
                                    'office_adress_postCode':'5555',
                                    'office_adress_City':'Hell',
                                 }");

            string patch = (@"
                                [
                                    { 'op': 'move', 'from': 'name.last_Name', 'path': 'familyName' },
                                    { 'op': 'move', 'from': 'name.first_Name', 'path': 'givenName' },
                                    { 'op': 'move', 'from': 'personal_adress_street', 'path': 'address[0].streetAddress' },
                                    { 'op': 'move', 'from': 'personal_adress_nr', 'path': 'address[0].postOfficeBoxNumber' },
                                    { 'op': 'move', 'from': 'personal_adress_postCode', 'path': 'address[0].postalCode' },
                                    { 'op': 'move', 'from': 'personal_adress_City', 'path': 'address[0].addressLocality' },
                                    { 'op': 'add', 'path': 'address[0].contactType','value':'private' },
                                    { 'op': 'move', 'from': 'office_adress_street', 'path': 'address[1].streetAddress' },
                                    { 'op': 'move', 'from': 'office_adress_nr', 'path': 'address[1].postOfficeBoxNumber' },
                                    { 'op': 'move', 'from': 'office_adress_postCode', 'path': 'address[1].postalCode' },
                                    { 'op': 'move', 'from': 'office_adress_City', 'path': 'address[1].addressLocality' },
                                    { 'op': 'add', 'path': 'address[1].contactType','value':'office' }                                   
                                ]"
                                );

           
             var result = await JsonPatchDocument.FromJson(patch).ApplyTo(originalDocument);
            
             Result will be :
                                "{  
                                       'familyName':'Dumont',
                                       'givenName':'Cedric',
                                       'address':[
                                          {
                                             'streetAddress':'heavenstreet',
                                             'postOfficeBoxNumber':'10',
                                             'postalCode':'4444',
                                             'addressLocality':'Paradise',
                                             'contactType':'private'
                                          },
                                          {
                                             'streetAddress':'badStreet',
                                             'postOfficeBoxNumber':'666',
                                             'postalCode':'5555',
                                             'addressLocality':'Hell',
                                             'contactType':'office'
                                          }
                                       ]
                                    }"
                                    

#### Usage example with Object

To Come...                                    
