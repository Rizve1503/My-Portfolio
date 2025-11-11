function Hero() {
  return (
    <section className="min-h-screen flex items-center justify-center px-6 py-24 md:py-32 bg-background-dark animate-fadeIn">
      <div className="max-w-7xl w-full grid grid-cols-1 md:grid-cols-2 items-center gap-16">
        {/* Left Side - Profile Image */}
        <div className="flex justify-center md:justify-end order-1 md:order-1">
          <div
            className="relative"
          >
            <div
              className="rounded-full h-52 w-52 object-cover shadow-xl ring-2 ring-[var(--accent)] ring-offset-4 ring-offset-[var(--background-dark)]"
              style={{
                boxShadow: '0 0 60px rgba(16, 185, 129, 0.3), 0 0 100px rgba(16, 185, 129, 0.15)'
              }}
            >
              {/* Placeholder for profile image */}
            </div>
          </div>
        </div>

        {/* Right Side - Text Content */}
        <div className="flex flex-col gap-6 text-center md:text-left order-2 md:order-2 max-w-lg">
          <h1 className="text-4xl md:text-5xl font-semibold tracking-tight text-text-primary">
            Rizve Ahmad
          </h1>
          
          <h2 className="text-lg md:text-xl text-[var(--text-secondary)] mb-4">
            .NET Software Engineer
          </h2>
          
          <p className="text-lg lg:text-xl text-text-secondary max-w-lg leading-relaxed">
            Crafting clean architecture & robust backend systems.
          </p>

          {/* CTA Buttons */}
          <div className="flex flex-col sm:flex-row gap-4 justify-center md:justify-start mt-6">
            <button 
              className="px-8 py-4 rounded-xl font-semibold text-white transition-all duration-300 hover:opacity-90 hover:shadow-lg"
              style={{ backgroundColor: '#10B981' }}
            >
              Download CV
            </button>
            
            <button 
              className="px-8 py-4 rounded-xl font-semibold border-2 transition-all duration-300 hover:bg-accent-hover"
              style={{ 
                color: '#10B981', 
                borderColor: '#10B981' 
              }}
            >
              View LinkedIn
            </button>
          </div>
        </div>
      </div>
    </section>
  );
}

export default Hero;
