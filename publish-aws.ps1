$s3Client = "s3://scrumpoker.com.br"
$cloudfrontID = ""

cd .\ScrumPoker_Client\
ng build --prod

aws cloudfront create-invalidation --distribution-id $cloudfrontID --paths "/*"
aws s3 sync .\dist\ $s3Client --delete --acl public-read

cd ..