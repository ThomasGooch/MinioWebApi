# MinioWebApi
 Minio Web Api POC

## docker Minio setup
docker run -p 9000:9000 -p 9001:9001 --name minio -v /Users/<USER>/Documents/minio_data:/data -e "MINIO_ROOT_USER=youraccesskey" -e "MINIO_ROOT_PASSWORD=yoursecretkey" minio/minio server /data --console-address ":9001"

go to the console at 9001 and create a "consent" bucket

## curls to use
curl -X POST "http://localhost:5189/api/pdf/upload"  -H "Content-Type: multipart/form-data" -F "file=@/Users/<USER>/Documents/medical.pdf"

curl -X GET "http://localhost:5189/api/pdf/download/medical.pdf" -o /User/<USER>/Documents/downloadMedical.pdf

curl -X DELETE "http://localhost:5189/api/pdf/delete/medical.pdf"