How To Run this project

1. update connection string in appsettings.json file inside EmsisoftTest folder
2. Run 'PM> add-migration MigrationV1' followed by 'PM> update-database' in the Package Management Console
3. Configure BackgroundWorker Project's HashDataModel.edmx
4. Go to Solution->properties->Startup Project, select Multiple Startup Project and select "start" in Action dropdown menu
5. Run project (or press Ctrl+F5)
