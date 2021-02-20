$s3Client = "s3://site.scrumpoker.com.br"

cd .\ScrumPoker_Client\
ng build --prod

aws s3 website sync .\dist\ $s3Client --delete --acl public-read

cd ..