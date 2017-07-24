import * as process from 'process';

// Show error in console instead of blocking dialog
process.on('uncaughtException', (error: Error) => {
    console.error(error);
});