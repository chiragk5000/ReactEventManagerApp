import React, { useEffect, useRef, useState } from 'react'
import { useAccount } from '../../lib/hooks/useAcounts'
import { Link, useSearchParams } from 'react-router';
import { Box, Button, Divider, Paper, Typography } from '@mui/material';
import { EmailRounded } from '@mui/icons-material';



export default function VerifyEmail() {
    const { verifyEmail } = useAccount();
    const [status, setStatus] = useState('Verifying');
    const [searchParams] = useSearchParams();
    const userId = searchParams.get('userId');
    const code = searchParams.get('code');

const hasRun = useRef(false as boolean);
    useEffect(() => {
        if (code && userId && !hasRun.current) {
            hasRun.current = true;
            verifyEmail.mutateAsync({ userId, code })
                .then(() => setStatus('verified'))
                .catch(() => setStatus('failed'))
        }

    }, [userId, code, verifyEmail])

    const getBody = () => {
        switch (status) {
            case 'verifying':
                return <Typography>Verifying...</Typography>
            case 'failed':
                return (
                    <Box display='flex' flexDirection='column' gap={2} justifyContent='center'>
                        <Typography>
                            Verification failed. You can try resending the verify link to your account
                        </Typography>
                        <Button>
                            Resend verification email
                        </Button>
                    </Box>
                );
            case 'verified':
                return (
                    <Box display='flex' flexDirection='column' gap={2} justifyContent='center'>
                        <Typography>
                            Email verified. You can try login
                        </Typography>
                        <Button component={Link} to="/login">
                            Go to login
                        </Button>
                    </Box>
                );
        }
    }

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
            <EmailRounded sx={{fontSize:100}} color='primary'/>
            <Typography gutterBottom variant='h3'>Email verification</Typography>
            <Divider/>
            {getBody()}
            </Paper>
    )
}
