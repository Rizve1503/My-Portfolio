function PurpleTheme() {
  return (
    <div className="min-h-screen flex items-center justify-center" style={{ backgroundColor: '#0B0D0F' }}>
      <div className="text-center space-y-8">
        {/* Profile Image Placeholder */}
        <div className="flex justify-center">
          <div className="w-32 h-32 rounded-full bg-gray-700 border-4" style={{ borderColor: '#A855F7' }}></div>
        </div>

        {/* Name */}
        <h1 className="text-5xl font-bold text-white">
          Rizve Ahmad
        </h1>

        {/* Role Subtitle */}
        <h2 className="text-2xl font-medium" style={{ color: '#94A3B8' }}>
          .NET Software Engineer
        </h2>

        {/* Tagline */}
        <p className="text-lg" style={{ color: '#94A3B8' }}>
          Crafting clean architecture & robust backend systems.
        </p>

        {/* CTA Buttons */}
        <div className="flex gap-4 justify-center pt-4">
          <button 
            className="px-6 py-3 rounded-lg font-semibold text-white transition-all duration-300"
            style={{ backgroundColor: '#A855F7' }}
            onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#C084FC'}
            onMouseLeave={(e) => e.currentTarget.style.backgroundColor = '#A855F7'}
          >
            Download CV
          </button>
          <button 
            className="px-6 py-3 rounded-lg font-semibold border-2 transition-all duration-300 hover:bg-opacity-10"
            style={{ color: '#A855F7', borderColor: '#A855F7' }}
            onMouseEnter={(e) => {
              e.currentTarget.style.color = '#C084FC';
              e.currentTarget.style.borderColor = '#C084FC';
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.color = '#A855F7';
              e.currentTarget.style.borderColor = '#A855F7';
            }}
          >
            View LinkedIn
          </button>
        </div>
      </div>
    </div>
  );
}

export default PurpleTheme;
