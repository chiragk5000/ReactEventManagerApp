import { Box, Typography, Grid2, Button } from "@mui/material";
import { CloudUpload } from "@mui/icons-material";
import { useDropzone } from "react-dropzone";
import { useCallback, useRef, useState, useEffect } from "react";
import Cropper, { ReactCropperElement } from "react-cropper";
import "cropperjs/dist/cropper.css"; // ✅ important: include cropper styles!



type Props = {
    uploadPhoto: (file: Blob) => void
    loading: boolean
}


export default function PhotoUploadWidget({ uploadPhoto, loading }: Props) {
    const [files, setFiles] = useState<object & { preview: string }[]>([]);
    //const [croppedImage, setCroppedImage] = useState<string | null>(null);
    const cropperRef = useRef<ReactCropperElement>(null);

    // cleanup code 
    useEffect(()=>{
        return ()=>{
            files.forEach(file=>URL.revokeObjectURL(file.preview))
        }
    },[files]);

    // 🧩 handle file drop
    const onDrop = useCallback((acceptedFiles: File[]) => {
        setFiles(
            acceptedFiles.map((file) =>
                Object.assign(file, {
                    preview: URL.createObjectURL(file),
                })
            )
        );
        //setCroppedImage(null);
    }, []);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        multiple: false,
        accept: { "image/*": [] },
    });

    // 🧹 clean up object URLs
    useEffect(() => {
        return () => files.forEach((file) => URL.revokeObjectURL(file.preview));
    }, [files]);

    // ✂️ crop handler
    // const handleCrop = () => {
    //     const cropper = cropperRef.current?.cropper;
    //     if (cropper) {
    //         const canvas = cropper.getCroppedCanvas();
    //         if (canvas) {
    //             setCroppedImage(canvas.toDataURL());
    //         }
    //     }
    // };

    const onCrop = useCallback(() => {
        const cropper = cropperRef.current?.cropper;
        cropper?.getCroppedCanvas().toBlob(blob => {
            uploadPhoto(blob as Blob)
        })

    }, [uploadPhoto])



    return (
        <Grid2 container spacing={3}>
            {/* Step 1 */}
            <Grid2 size={4}>
                <Typography variant="overline" color="secondary">
                    Step 1: Add Photo
                </Typography>
                <Box
                    {...getRootProps()}
                    sx={{
                        border: "3px dashed #eee",
                        borderColor: isDragActive ? "green" : "#eee",
                        borderRadius: "5px",
                        p: 3,
                        textAlign: "center",
                        height: "280px",
                        cursor: "pointer",
                    }}
                >
                    <input {...getInputProps()} />
                    <CloudUpload sx={{ fontSize: 80, color: "gray" }} />
                    <Typography variant="h6">Drop or select an image</Typography>
                </Box>
                
            </Grid2>

            {/* Step 2 */}
            <Grid2 size={4}>
                <Typography variant="overline" color="secondary">
                    Step 2: Resize Photo
                </Typography>
                {files[0]?.preview && (
                    // <Box
                    //     sx={{
                    //         width: "100%",
                    //         maxWidth: 300,
                    //         overflow: "hidden",
                    //         display: "flex",
                    //         justifyContent: "center",
                    //         alignItems: "center",
                    //         mt: 2,
                    //     }}
                    // >
                        <Cropper
                            src={files[0]?.preview}
                            style={{ height: 300, width: "100%" }}
                            initialAspectRatio={1}
                            aspectRatio={1}
                            guides={false}
                            viewMode={1}
                            background={false}
                            ref={cropperRef}
                            preview=".img-preview"
                        />
                    // </Box>
                )}
                {/* {files[0]?.preview && (
          <Button
            onClick={handleCrop}
            variant="contained"
            sx={{ mt: 2, borderRadius: 3 }}
          >
            Crop Image
          </Button>
        )} */}
            </Grid2>

            {/* Step 3 */}
            <Grid2 size={4}>
                <Typography variant="overline" color="secondary">
                            Step 3: Preview and Upload
                        </Typography>
                {files[0]?.preview && (
                    <>
                        
                        <div className="img-preview" style={{width:300,height:300,overflow:'hidden'}}/>
                        {/* <Box
                            className="img-preview"
                            sx={{
                                width: 300,
                                height: 300,
                                overflow: "hidden",
                                borderRadius: "10px",
                                border: "2px solid #ccc",
                                mt: 2,
                            }}
                        />
                        {croppedImage && (
                            <Box mt={2}>
                                <img
                                    src={croppedImage}
                                    alt="Cropped"
                                    style={{
                                        width: 300,
                                        height: 300,
                                        borderRadius: "10px",
                                        objectFit: "cover",
                                    }}
                                />
                            </Box>
                        )} */}
                        <Button
                            sx={{ mt: 2 }}
                            onClick={onCrop}
                            variant={'contained'}
                            color={'secondary'}
                            disabled={loading}
                        >
                            Upload Photo
                        </Button>
                    </>
                )}
            </Grid2>
        </Grid2>
    );
}
