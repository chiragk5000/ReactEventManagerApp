import { Button, Paper, Typography } from "@mui/material";
import { useAccount } from "../../lib/hooks/useAcounts"
import { Check } from "@mui/icons-material";

type Props = {
    email?: string
}

export default function RegisterSucess({ email }: Props) {
    const { resendConfirmationEmail } = useAccount();

    if (!email) return null;

    return (
        <Paper sx={{
            height: 400,
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            p: 6
        }}
        >
            <Check sx={{ fontSize: 100 }} color="primary" />
            <Typography gutterBottom variant="h3">
                You Have sucesfuly registered !!!
            </Typography>
            <Typography gutterBottom variant="h3">
                Please check your email to confirm your Account            
                </Typography>
                <Button fullWidth onClick={()=> resendConfirmationEmail.mutate({email})}>
                    Resend Confiramtion Email

                </Button>
        </Paper>
    )
}
