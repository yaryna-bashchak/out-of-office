import './App.css';
import { ThemeProvider } from '@mui/material/styles';
import { createTheme } from '@mui/material';
import Layout from './Layout';

function App() {
    const theme = createTheme({
        palette: {
            primary: {
                main: '#1976d2',
            },
            secondary: {
                main: '#dc004e',
            },
        },
    });

    return (
        <>
            <ThemeProvider theme={theme}>
                <Layout />
            </ThemeProvider >
        </>
    );
}

export default App;