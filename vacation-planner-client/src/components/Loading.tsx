export function Loading() {
    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100">
            <div className="text-center">
                <div className="spinner-border text-secondary" role="status"></div>
                <div className="text-secondary mt-2">Laadimine...</div>
            </div>
        </div>
    );
}